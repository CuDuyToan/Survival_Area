using System;
using UnityEngine;
using PlayerState;


public class PlayerController : Creature, IPlayer
{
    #region Private Members
    [Header("Player")]
    [SerializeField] private ItemStack _currentItem = null;

    #region current item

    private float BonusDame
    {
        get
        {
            if(_CurrentItem == null) return 0;

            if (_CurrentItem._Item == null) return 0;

            if (_CurrentItem._Item is ToolSO tool)
            {
                return tool.BonusDame;
            }

            if(_CurrentItem._Item is WeaponSO weapon)
            {
                return weapon.BonusDame;
            }

            return 0;
        }
    }

    public ItemStack _CurrentItem
    {
        set
        {
            if(_currentItem == value) return;
            _currentItem = value;
        }
        get
        {
            if (_currentItem == null) return null;

            if (_currentItem._Quantity == 0)
            {
                _currentItem = null;
            }

            return _currentItem;
        }
    }

    private void SwitchItemOnHand(ItemStack itemStack)
    {
        foreach (Transform child in _Hand.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (itemStack == null || _CurrentItem == itemStack)
        {
            _CurrentItem = null;
            return;
        }

        CurrentItemIsTool(itemStack);

        CurrentItemIsWeapon(itemStack);

        CurrentItemIsStructure(itemStack);
    }

    private void CurrentItemIsTool(ItemStack itemStack)
    {
        if (itemStack._Item is ToolSO tool)
        {
            foreach (Transform child in _Hand.transform)
            {
                if (tool.ToolPrefab.name == child.name)
                {
                    child.gameObject.SetActive(true);
                }
            }

            _CurrentItem = itemStack;
        }
    }

    private void CurrentItemIsWeapon(ItemStack itemStack)
    {
        if (itemStack._Item is WeaponSO weapon)
        {
            foreach (Transform child in _Hand.transform)
            {
                if (weapon.WeaponPrefab.name == child.name)
                {
                    child.gameObject.SetActive(true);
                }
            }

            _CurrentItem = itemStack;
        }
    }

    private void CurrentItemIsStructure(ItemStack itemStack)
    {
        if(itemStack._Item is StructureSO)
        {
            _CurrentItem = itemStack;
        }
    }

    #endregion current item

    private IPlayerState _currentState;

    
    [SerializeField] private SphereCollider _interactiveRange;

    [SerializeField] private Vector3 _moveOffset = Vector3.zero;
    [HideInInspector] public Vector3 _MoveOffset 
    {
        set {if(_moveOffset != Vector3.zero) _moveOffset = value; }
        get { return _moveOffset; }
    }

    [SerializeField] private LayerMask _moveLayer;
    [SerializeField] private LayerMask _targetLayer;

    #endregion

    #region Public Members

    public InventoryPlayer _inventory;

    private float maxWater
    {
        get
        {
            if(_creatureSO is PlayerSO player)
            {
                return player.MaxWater;
            }

            return 0;
        }
    }
    [Header("water")]
    [SerializeField] private float water;
    public float Water
    {
        set
        {
            if (value > maxWater) water = maxWater;
            else if(value < 0) water = 0;
            else water = value;
        }
        get
        {
            return water;
        }
    }

    [SerializeField] private IndexBar waterBar;

    public override void Eat(FoodSO food)
    {
        base.Eat(food);

        Water += food.WaterPoint;
        _Health += food.HealthPoint;
    }

    private void IncreaseThirsty()
    {
        if(_creatureSO is PlayerSO playerSO)
        {
            Water -= playerSO.WaterDecrease;

            if (waterBar != null) waterBar.SetValue(Water);
        }
    }

    public override void RecoveryStamina(float recoverySpeed)
    {
        if (_Stamina < MaxStamina)
        {
            float recoveryValue = _creatureSO.RecoveryValue * recoverySpeed;

            if (Water >= recoveryValue * 0.125f && _Food >= recoveryValue * 0.05f)
            {
                _Food -= recoveryValue * 0.05f;
                Water -= recoveryValue * 0.125f;
                _Stamina += recoveryValue;
            }

            if (_staminaBar != null) _staminaBar.SetValue(_Stamina);
        }
    }

    public override void Recovery()
    {
        base.Recovery();

        IncreaseThirsty();
    }

    public float WeightRate
    {
        get
        {
            return (_inventory.TotalWeight / _creatureSO.WeightCapacity) * 100;
        }
    }

    [Header("Weight bar")]
    [SerializeField] IndexBar weightBar;
    [Header("Hand")]
    public GameObject _Hand;
    //[Header("target")]
    //[SerializeField] private GameObject _targetObj;
    public override GameObject Target
    {
        set
        {
            if (value == this.gameObject) return;

            target = value;
            if (target == null) CanInteract = false;
            ResetTarget();
        }
        get
        {
            if (target == null) CanInteract = false;
            ResetTarget();
            return target;
        }
    }

    private void ResetTarget()
    {
        if (target == null) return;

        if (target.GetComponent<Collider>().enabled == false)
        {
            Collider col = target.GetComponent<Collider>();
            if (col != null && !col.enabled)
            {
                target = null;
            }
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        if(_creatureSO is PlayerSO playerSO)
        {
            Water = playerSO.MaxWater;
        }


        _foodBar.Min = 0;
        _foodBar.Max = MaxFood;
        _foodBar.SetValue(_Food);

        _staminaBar.Min = 0;
        _staminaBar.Max = _creatureSO.MaxStamina;
        _staminaBar.SetValue(_Stamina);

        weightBar.Min = 0;
        weightBar.Max = _creatureSO.WeightCapacity;
        weightBar.SetValue(_inventory.TotalWeight);

        waterBar.Min = 0;
        waterBar.Max = Water;
        waterBar.SetValue(Water);
    }

    protected override void Start()
    {
        base.Start();


        InventoryPlayer.OnSelectItem += SwitchItemOnHand;


        this.SwitchState(new Idle());
    }

    private void OnDestroy()
    {
        InventoryPlayer.OnSelectItem -= SwitchItemOnHand;
    }

    public void SwitchState(IPlayerState newState)
    {
        if (newState == _currentState) return;

        //Debug.LogWarning("Player switch state" + newState.ToString());

        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    protected override void Update()
    {
        base.Update();

        _currentState.Update(this);

        weightBar.SetValue(WeightRate);
    }

    #region Action

    public void ResetTrigger()
    {
        _animator.ResetTrigger("Walk");
        //_animator.ResetTrigger("Run");
        _animator.ResetTrigger("Sprint");
        _animator.ResetTrigger("Idle");
    }

    private void OnWalk()
    {
        _animator.SetBool("Run", false);
        Walk();
    }

    private void OnRun()
    {
        Run();
    }

    public void Walk()
    {
        ResetTrigger();

        

        if (WeightRate >= 100)
        {
            _agent.speed = 0;
            _animator.SetTrigger("Idle");
            return;
        }

        

        _agent.speed = _creatureSO.WalkSpeed;
        _animator.SetTrigger("Walk");

        if (_animator.GetBool("Run"))
        {
            _agent.speed = _creatureSO.RunSpeed;
            return;
        }
    }

    public void Run()
    {
        ResetTrigger();
        if (_Stamina <= 0 || WeightRate >= 85)
        {
            Walk();
            return;
        }

        _agent.speed = _creatureSO.RunSpeed;
        _animator.SetBool("Run", true);
    }

    public void Sprint()
    {
        if (_Stamina <= 0 || WeightRate >= 85)
        {
            Walk();
            return;
        }

        if (_creatureSO is PlayerSO playerSO)
        {
            _agent.speed = playerSO.SprintSpeed;
            _animator.SetTrigger("Sprint");
        }
    }

    

    public void Attack()
    {
        if(_CurrentItem == null)
        {
            _animator.SetTrigger("Punch");
        }
        else
        {
            if (_CurrentItem._Item != null &&
            (_CurrentItem._Item is ToolSO ||
            _CurrentItem._Item is WeaponSO))
            {
                _animator.SetTrigger("Attack");
            }
            else if (_CurrentItem == null)
            {
                _animator.SetTrigger("Punch");
            }
        }

        
    }

    public void DealDamage()
    {
        if (Target == null) return;

        Creature creature = Target.GetComponent<Creature>();

        if (creature != null) creature.TakeDame(_creatureSO.Damage + BonusDame, this.gameObject);
    }

    public void SelfHarm(float amount)
    {
        TakeDame(amount, Target);
    }

    public void Mining()
    {
        if (_CurrentItem != null && _CurrentItem._Item is ToolSO toolData)
        {
            if(toolData.ToolTag != EToolType.Sickle)
            _animator.SetTrigger("Mining");
        }
        else
        {
            if (Target.GetComponent<Resource>().ExploitByHand == false)
            {
                Debug.Log("can mining");
            }
            else
            {
                _animator.SetTrigger("Punch");
            }
        }
    }

    public void Gathering()
    {
        _animator.SetTrigger("Gathering");
    }

    public void Exploit()
    {
        if (Target == null) return;

        //DecreaseDurabilityCurrentItem();

        Resource resource = Target.GetComponent<Resource>();
        if (resource != null) resource.Exploited(this);
    }

    public void DecreaseDurabilityCurrentItem()
    {
        if (this._CurrentItem == null) return;
        if (this._CurrentItem._Item == null) return;

        if(this._CurrentItem._Item is ToolSO)
        {
            this._CurrentItem._Durability -= 1;
        }

        if(this._CurrentItem == null)
        {
            SwitchItemOnHand(null);
            SwitchState(new Idle());
        }
    }

    #endregion


    #region Event

    private void OnEnable()
    {
        InputHandle.OnIdle += Idle;

        InputHandle.OnSetDestination += Move;
        InputHandle.OnSelectTarget += SelectTarget;

        InputHandle.OnWalk += OnWalk;
        InputHandle.OnRun += OnRun;
        InputHandle.OnSprint += Sprint;
    }

    private void OnDisable()
    {
        InputHandle.OnIdle -= Idle;

        InputHandle.OnSetDestination -= Move;
        InputHandle.OnSelectTarget -= SelectTarget;

        InputHandle.OnWalk -= OnWalk;
        InputHandle.OnRun -= OnRun;
        InputHandle.OnSprint -= Sprint;

    }

    private void Idle(bool condition)
    {
        if(condition == true) SwitchState(new Idle());
    }

    private void Sprint(bool active)
    {
        if (active == true)
        {
            SwitchState(new Sprint());
        }
        else
        {
            Walk();
            SwitchState(new Move());
        }
    }

    private void Move()
    {
        SwitchState(new Move());
    }

    private void SelectTarget(GameObject target)
    {
        if (_CurrentItem != null)
        {
            if(_CurrentItem._Item is StructureSO)
            {
                return;
            }
        }

        Target = target;

        CanInteract = false;
        if (Target != null) SwitchState(new FindTarget());
    }

    #endregion

    //private void TargetObj()
    //{
    //    RaycastToTarget();
    //    CanInteract = false;
    //    if (Target != null) SwitchState(new FindTarget());
    //}

    public void RaycastToTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// tao raycast moi
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _targetLayer))
        {
            if(hit.collider.isTrigger == false)
            {
                Debug.LogWarning("this trigger in collider not enable : "+ hit.collider.gameObject.name);
            }else if (hit.collider.isTrigger == true)
            {
                Target = hit.collider.gameObject;
            }
        }
        else Target = null;
    }

    public void RaycastToDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// tao raycast moi
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _moveLayer))
        {
            _moveOffset = hit.point;
        }
        else _moveOffset = Vector3.zero;
    }

    

    public void StopMovement(bool active)
    {
        _agent.isStopped = active;
    }

    public static event Action OnGameOver;
    public override void Die()
    {
        base.Die();
        OnGameOver?.Invoke();
    }

    public override void TakeDame(float amount, GameObject source)
    {
        if (!_canTakeDamage)
            return;

        _Health -= amount / (1 + _creatureSO.Armor / (50f - _creatureSO.ReduceDamage));
        _animator.SetTrigger("Get hit");
        _healthBar.SetValue(_Health);
    }
}
