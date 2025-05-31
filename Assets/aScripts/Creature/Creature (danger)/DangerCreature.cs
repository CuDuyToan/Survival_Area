using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using DangerCreatureState;
using Unity.VisualScripting;

public class DangerCreature : Creature , IDangerCreature
{
    private IDangerCreatureState _currentState;

    public EGroupingType _groupType
    {
        get
        {
            if(_creatureSO is DangerCreatureSO enemydata)
            {
                return enemydata.GroupingType;
            }
            return EGroupingType.SoloBased;
        }
    }

    #region Collider Trigger [OFF]

    [Header("Detection and Attack Ranges : trigger[off]")]

    [SerializeField] private SphereCollider _patrolCollider;

    [Tooltip("Scope")]
    [SerializeField] private SphereCollider _searchCollider;


    private float _patrolRange => (_patrolCollider ? _patrolCollider.radius : 0);

    private float _searchingRange => _searchCollider ? _searchCollider.radius : 0;

    #endregion Collider Trigger [OFF]


    #region Collider Trigger [ON]
    [Header("Detection and Attack Ranges : trigger[ON]")]
    [Tooltip("_isPerception")]
    [SerializeField] private SphereCollider _visionCollider;
    [Tooltip("_isPerception")]
    [SerializeField] private SphereCollider _sensingCollider;

    [Tooltip("_isAbleToAttack")]
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private Collider _attackZone;

    #endregion

    #region View
    [Header("View")]
    [SerializeField] private Transform _eye;

    [SerializeField] private LayerMask _obstructionLayer;

    //public Creature _target;

    #endregion

    #region processing in animation

    public void FlagAgentStopOn()
    {
        _agent.isStopped = true;
    }

    public void FlagAgentStopOff()
    {
        _agent.isStopped = false;
    }

    public void InjureTheTarget()
    {
        StartCoroutine(ActivateHitbox());
    }

    private IEnumerator ActivateHitbox()
    {
        _attackZone.gameObject.SetActive(true);  // Hiện hitbox
        yield return new WaitForSeconds(0.01f); // Chờ 0.1s
        _attackZone.gameObject.SetActive(false); // Ẩn đi
    }

    public void Grab()
    {
        StartCoroutine(ActivateHitbox());
    }

    #endregion

    protected override void Start()
    {
        base.Start();

        _attackZone.gameObject.SetActive(false);
        SwitchState(new Wadering()); // Bắt đầu với trạng thái tuần tra
    }

    public void SwitchState(IDangerCreatureState newState)
    {
        if (_currentState != null && _currentState.GetType() == newState.GetType()) return;

        //Debug.Log($"{this.name} {newState.ToString()}");

        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    protected override void Update()
    {
        base.Update();

        _currentState.Update(this);
    }

    #region Animation

    //public void Idle(bool state)
    //{
    //    _animator.SetBool("Idle", state);
    //    if (state) _agent.speed = 0f;
    //}

    //public void Walk(bool state)
    //{
    //    if (state)
    //    {
    //        _animator.SetTrigger("Walk");
    //        _agent.speed = _creatureSO.WalkSpeed;
    //    }
    //    else
    //    {
    //        _animator.ResetTrigger("Walk");
    //    }
    //}

    public void Walk()
    {
        Agent.speed = _creatureSO.WalkSpeed;
        _animator.SetTrigger("Walk");
    }

    public void Sprint()
    {
        Agent.speed = _creatureSO.RunSpeed;
        _animator.SetTrigger("Sprint");
    }

    public void Howl()
    {
        _animator.SetTrigger("Howl");
    }

    public void FinishHowl()
    {
        _animator.ResetTrigger("Howl");
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    #endregion

    public bool DetectCreature()
    {
        if (targetList.Count <= 0) return false;
        foreach (GameObject prey in targetList)
        {
            Vector3 preyPos = new Vector3(prey.transform.position.x, prey.transform.position.y + 1, prey.transform.position.z);
            if (prey == null) continue;

            Vector3 directionToPrey = (preyPos - _eye.position).normalized;

            float distanceToPrey = Vector3.Distance(_eye.position, preyPos);

            float angleToPrey = Vector3.Angle(_eye.forward, directionToPrey);

            if (!Physics.Raycast(_eye.position, directionToPrey, distanceToPrey, _obstructionLayer) && distanceToPrey < _visionCollider.radius)
            {
                if(_creatureSO is DangerCreatureSO enemyData)
                {
                    if (angleToPrey < enemyData.FieldOfViewAngle / 2)
                    {
                        Target = prey.gameObject;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void TakeDame(float amount, GameObject source)
    {
        base.TakeDame(amount, source);

        if(Target == null)
        {
            Target = source;
            SwitchState(new ChaseState());
        }
    }

    public bool Agitated()
    {
        if (Target == null) return false;
        Vector3 preyPos = new Vector3(Target.transform.position.x, Target.transform.position.y + 1, Target.transform.position.z);

        Vector3 directionToPlayer = (preyPos - _eye.position).normalized;

        float distanceToPlayer = Mathf.Min(Vector3.Distance(_eye.position, preyPos), _visionCollider.radius);

        float angleToPlayer = Vector3.Angle(_eye.forward, directionToPlayer);

        if (!Physics.Raycast(_eye.position, directionToPlayer, distanceToPlayer, _obstructionLayer) && (distanceToPlayer < _visionCollider.radius))
        {
            if (_creatureSO is DangerCreatureSO enemyData)
            {
                if (angleToPlayer < 180 + enemyData.FieldOfViewAngle / 2)
                {
                    Debug.DrawRay(_eye.position, directionToPlayer * distanceToPlayer, Color.green);
                    return true;
                }
            }
            Debug.DrawRay(_eye.position, directionToPlayer * distanceToPlayer, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(_eye.position, directionToPlayer * distanceToPlayer, Color.red);
            return false;
        }
    }

    public bool ReachedDestination() => !_agent.pathPending && _agent.remainingDistance < 0.5f;

    #region Move Random
    public void RandomDestination()
    {
        float moveDistance = _patrolRange;
        Vector3 randomPos = transform.position + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
    }

    public Vector3 SearchInArea(Vector3 lostLocation)
    {
        float moveDistance = _searchingRange;
        Vector3 randomPos = lostLocation + new Vector3(Random.Range(-moveDistance, moveDistance), 0, Random.Range(-moveDistance, moveDistance));
        _agent.SetDestination(randomPos);
        return randomPos;
    }
    #endregion
}

