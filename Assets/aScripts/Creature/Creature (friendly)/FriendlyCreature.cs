using FriendlyCreatureState;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendlyCreature : Creature, IFriendlyCreature
{
    private IFriendlyCreatureState currentState;

    #region Collider Trigger [OFF]

    [Header("Detection and Attack Ranges : trigger[off]")]

    [SerializeField] private SphereCollider patrolCollider;

    private float patrolRange => (patrolCollider ? patrolCollider.radius : 0);

    #endregion Collider Trigger [OFF]

    public void SwitchState(IFriendlyCreatureState newState)
    {
        if (newState == currentState) return;

        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
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

    #region animation
    public void Run()
    {
        _agent.speed = _creatureSO.RunSpeed;
        _animator.SetTrigger("Run");
    }

    public void Walk()
    {
        _agent.speed = _creatureSO.WalkSpeed;
        _animator.SetTrigger("Walk");
    }
    #endregion animation

    [SerializeField] private SphereCollider safeCollider;
    public float SafeDistance => safeCollider.radius;
    public void Escape()
    {
        Vector3 direction = (Target.transform.position - this.transform.position).normalized;
        Vector3 newPosition = this.transform.position - direction * SafeDistance;


        _agent.SetDestination(newPosition);
    }

    public void RandomDestination()
    {
        float moveDistance = patrolRange;
        Vector3 randomPos = transform.position + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
    }

    public override void TakeDame(float amount, GameObject source)
    {
        base.TakeDame(amount, source);

        //Target = source;
        SwitchState(new Escape());
    }
}
