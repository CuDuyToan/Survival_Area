using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour , ICreature
{
    [Header("ScriptAble Object")]
    public CreatureSO _creatureSO;

    #region Private

    [SerializeField] private float _health;
    [SerializeField] private float _food;
    [SerializeField] private float _stamina;

    private Creature_SpawnDeadbody _spawnDeadbody;

    #endregion


    #region Public Member
    [Header("index bar")]
    public IndexBar _healthBar;
    public IndexBar _foodBar;
    public IndexBar _staminaBar;

    [Header("Target")]
    [HideInInspector] protected List<GameObject> targetList = new List<GameObject>();
    public List<GameObject> TargetList
    {
        set
        {
            targetList = value;
        }
        get
        {
            foreach (GameObject obj in targetList)
            {
                if (obj == null) targetList.Remove(obj);
            }

            return targetList;
        }
    }

    [SerializeField] protected GameObject target;
    public virtual GameObject Target
    {
        set
        {
            if (value == this.gameObject) return;

            target = value;
        }
        get
        {
            if(target != null)
            {
                Creature creature = target.GetComponent<Creature>();
                if (creature != null)
                {
                    if (creature.IsDead) target = null;
                    return target;
                }

                Structure structure = target.GetComponent<Structure>();
                if (structure != null)
                {
                    if (structure._Health <= 0) target = null;
                    return target;
                }
            }

            return target;
        }
    }

    [SerializeField] private bool canInteract = false;
    public bool CanInteract
    {
        set
        {
            canInteract = value;
        }
        get
        {
            if (Target == null) canInteract = false;
            return canInteract;
        }
    }

    [SerializeField] private bool isPerception = false;
    public bool IsPerception
    {
        set
        {
            isPerception = value;
        }
        get
        {
            if (this.Target == null)
            {
                IsPerception = false;
            }
            return isPerception;
        }
    }

    #endregion

    #region Protected Members
    protected Animator _animator;
    public Animator _Animator => _animator;
    protected NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [SerializeField] protected bool _canTakeDamage = true;

    #endregion

    public float _MovementSpeed => _agent.speed;

    #region Health & Hunger
    public float MaxHealth
    {
        get
        {
            return _creatureSO.MaxHealth;
        }
    }

    public float _Health
    {
        set 
        { 
            if (value > MaxHealth) _health = MaxHealth;
            else if(value < 0) _health = 0;
            else _health = value;
        }
        get
        {
            return _health;
        }
    }

    public float MaxFood
    {
        get
        {
            return _creatureSO.MaxFood;
        }
    }

    public float _Food
    {
        set
        {
            if (value > MaxFood) _food = MaxFood;
            else if (value < 0) _food = 0;
            else _food = value;
        }
        get
        {
            return _food;
        }
    }

    public float MaxStamina
    {
        get
        {
            return _creatureSO.MaxStamina;
        }
    }

    public float _Stamina
    {
        set
        {
            if (_stamina > MaxStamina) _stamina = MaxStamina;
            else _stamina = value;
        }
        get
        {
            return _stamina;
        }
    }

    public void IncreaseHunger()
    {
        float decreaseValue = _creatureSO.FoodDecrease;

        if (_foodBar != null) _foodBar.SetValue(_Food);

        if(_Food >= decreaseValue)
        {
            _Food -= decreaseValue;
        }
        else if(_Food <= decreaseValue)
        {
            _Health -= decreaseValue * 1.25f;
        }
    }

    public virtual void Recovery()
    {
        IncreaseHunger();

        RecoveryHealth();
        RecoveryStamina(1);
    }

    public void RecoveryHealth()
    {
        if (_Health >= MaxHealth || IsDead) return;

        float recoveryValue = _creatureSO.RecoveryValue;

        if (_Food >= recoveryValue && _Food > MaxHealth * 0.5f)
        {
            _Health += recoveryValue;
            _Food -= recoveryValue;
        }

        if (_healthBar != null) _healthBar.SetValue(_Health);
    }

    public void RestoreHealth(float amount)
    {
        _Health += amount;
    }

    public bool IsDead
    {
        get
        {
            return _Health <= 0;
        }
    }

    public virtual void Eat(FoodSO food)
    {
        _Food += food.FoodPoint;
    }
    #endregion



    public void DecreaseStamina(float staminaDecrease)
    {
        _Stamina -= staminaDecrease;
        _staminaBar.SetValue(_Stamina);
    }

    public virtual void RecoveryStamina(float recoverySpeed)
    {
        if (_Stamina >= MaxStamina) return;


        float recoveryValue = _creatureSO.RecoveryValue * recoverySpeed;

        if (_Food >= recoveryValue && _Food >= 0)
        {
            _Food -= recoveryValue;
            _Stamina += recoveryValue;
        }

        if (_staminaBar != null) _staminaBar.SetValue(_Stamina);
    }

    //public void RestoreStamina()
    //{
        
    //}

    #region dame & takedame
    public virtual void TakeDame(float amount, GameObject source)
    {
        if (!_canTakeDamage)
            return;

        _Health -= amount / (1 + _creatureSO.Armor / (50f - _creatureSO.ReduceDamage));
        _animator.SetTrigger("Get hit");
        _healthBar.SetValue(_Health);

        if(Target == null)
        {
            Target = source;
        }

    }

    public virtual void Die()
    {
        _spawnDeadbody.enabled = true;
    }

    #endregion

    protected virtual void Awake()
    {
        SetIndex();

        _spawnDeadbody = GetComponent<Creature_SpawnDeadbody>();

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();


        //InvokeRepeating("IncreaseHunger", 0, 1);
        InvokeRepeating("Recovery", 0, 1);

        _healthBar.Min = 0;
        _healthBar.Max = MaxHealth;
        _healthBar.SetValue(_Health);
    }

    protected virtual void Start()
    {
        

    }

    protected virtual void Update()
    {
        if (IsDead)
        {
            Die();
        }
    }

    public virtual void SetIndex()
    {
        _Food = MaxFood;
        _Health = MaxHealth;
        _Stamina = MaxStamina;
    }

    #region action

    public void Idle()
    {
        _agent.speed = 0;
        _animator.SetTrigger("Idle");
    }

    public void MoveToPoint(Vector3 destination)
    {
        if (destination == Vector3.zero)
        {
            return;
        }
        _agent.SetDestination(destination);
    }

    public bool IsPathComplete() => _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending;
    #endregion

    public void RotateToTarget()
    {
        if (Target == null) return;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        direction.y = 0;
        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
    }
}
