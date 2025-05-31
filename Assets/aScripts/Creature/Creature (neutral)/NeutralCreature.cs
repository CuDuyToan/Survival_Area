using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeutralCreatureState;

public class NeutralCreature : Creature
{
    private INeutralCreatureState currentState;

    #region Collider Trigger [OFF]

    [Header("Detection and Attack Ranges : trigger[off]")]

    [SerializeField] private SphereCollider patrolCollider;

    private float patrolRange => (patrolCollider ? patrolCollider.radius : 0);

    #endregion Collider Trigger [OFF]

    public void SwitchState(INeutralCreatureState newState)
    {
        if (currentState != null && newState.GetType() == currentState.GetType()) return;

        //Debug.LogWarning("creature switch state " + newState.ToString());

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

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Charge()
    {
        _agent.speed = _creatureSO.RunSpeed * 0.75f;
        _Animator.SetTrigger("Charge");
    }

    public void InjureTheTarget()
    {
        StartCoroutine(ActivateHitbox());
    }

    [SerializeField] private Collider attackZone;
    private IEnumerator ActivateHitbox()
    {
        attackZone.gameObject.SetActive(true);  // Hiện hitbox
        yield return new WaitForSeconds(0.01f); // Chờ 0.1s
        attackZone.gameObject.SetActive(false); // Ẩn đi
    }
    #endregion animation

    [SerializeField] private SphereCollider safeCollider;
    public float SafeDistance => safeCollider.radius;

    public bool isScared => this._Health <= this.MaxHealth * 0.3f;

    public void Escape()
    {
        Vector3 direction = (Target.transform.position - this.transform.position).normalized;
        Vector3 newPosition = this.transform.position - direction * SafeDistance;

        _agent.SetDestination(newPosition);
    }

    #region move random

    public void RandomDestination()
    {
        float moveDistance = patrolRange;
        Vector3 randomPos = transform.position + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
    }

    [Tooltip("Scope")]
    [SerializeField] private SphereCollider _searchCollider;

    private float searchingRange => _searchCollider ? _searchCollider.radius : 0;
    public Vector3 SearchInArea(Vector3 lostLocation)
    {
        float moveDistance = searchingRange;
        Vector3 randomPos = lostLocation + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
        return randomPos;
    }

    #endregion move random

    public override void TakeDame(float amount, GameObject source)
    {
        GameObject target = Target;

        base.TakeDame(amount, source);

        if(target == null)
        {
            if (Target != null) SwitchState(new Chase());
        }

    }

    #region vision
    [Header("vision")]
    [SerializeField] private Transform eye;
    [SerializeField] private LayerMask obstructionLayer;
    [Tooltip("_isPerception")]
    [SerializeField] private SphereCollider visionCollider;
    public bool Agitated()
    {
        if (Target == null) return false;
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y + 1, Target.transform.position.z);

        Vector3 directionToPlayer = (targetPos - eye.position).normalized;

        float distanceToPlayer = Mathf.Min(Vector3.Distance(eye.position, targetPos), visionCollider.radius);

        float angleToPlayer = Vector3.Angle(eye.forward, directionToPlayer);

        if (!Physics.Raycast(eye.position, directionToPlayer, distanceToPlayer, obstructionLayer) && (distanceToPlayer < visionCollider.radius))
        {
            if (_creatureSO is NeutralCreatureSO creatureData)
            {
                if (angleToPlayer < 180 + creatureData.FieldOfViewAngle / 2)
                {
                    Debug.DrawRay(eye.position, directionToPlayer * distanceToPlayer, Color.green);
                    return true;
                }
            }
            Debug.DrawRay(eye.position, directionToPlayer * distanceToPlayer, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(eye.position, directionToPlayer * distanceToPlayer, Color.red);
            return false;
        }
    }

    #endregion vision
}
