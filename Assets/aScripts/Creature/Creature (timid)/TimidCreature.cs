using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimidCreatureState;

public class TimidCreature : Creature
{
    ITimidCreatureState currentState;

    public void SwitchState(ITimidCreatureState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType()) return;

        //Debug.Log($"{this.name} : {newState}");

        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }


    [SerializeField] private SphereCollider safeCollider;
    public float SafeDistance => safeCollider.radius;

    public bool isSafe
    {
        get
        {
            if (Target == null) return true;

            return Vector3.Distance(this.transform.position, this.Target.transform.position) > this.SafeDistance;
        }
    }

    public void Escape()
    {
        Vector3 direction = (Target.transform.position - this.transform.position).normalized;
        Vector3 newPosition = this.transform.position - direction * SafeDistance;


        _agent.SetDestination(newPosition);
    }

    public override void TakeDame(float amount, GameObject source)
    {
        base.TakeDame(amount, source);


    }


    [SerializeField] private SphereCollider patrolCollider;
    private float patrolRange => (patrolCollider ? patrolCollider.radius : 0);
    public void RandomDestination()
    {
        float moveDistance = patrolRange;
        Vector3 randomPos = transform.position + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
    }

    public void Walk()
    {
        _agent.speed = _creatureSO.WalkSpeed;
        _animator.SetTrigger("Walk");
    }

    public void Run()
    {
        _agent.speed = _creatureSO.RunSpeed;
        _animator.SetTrigger("Run");
    }

    protected override void Start()
    {
        base.Start();

        SwitchState(new Wadering());
    }

    protected override void Update()
    {
        base.Update();

        currentState.Update(this);
    }
}
