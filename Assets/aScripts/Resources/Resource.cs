using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Resource : MonoBehaviour
{
    [SerializeField] public ResourceSO _resourceSO;
    #region protected

    protected NavMeshObstacle _obstacle;

    #endregion

    #region Private Member
    [SerializeField] private float _health;
    public float _Health
    {
        set
        {
            if ( value < 0) _health = 0;
            else _health = value;
        }
        get
        {
            return _health;
        }
    }

    //private bool _isCanRecovery;
    public bool _IsCanRecovery => _resourceSO.IsCanRecovery;

    [SerializeField] private float _timeCount;
    [HideInInspector] public bool isCounting = false;
    public float _TimeCount
    {
        set
        {
            if(value < 0) _timeCount = 0;
            else _timeCount = value;
        }
        get
        {
            return _timeCount;
        }
    }

    private Animation _animation;

    [SerializeField, Tooltip("the display of resources in the game environment")]
    public GameObject _resourceDisplay;
    [SerializeField] private GameObject _displayGroup;

    //audio
    private AudioSource audioSource;

    #endregion

    #region Public Member

    public EExploitType ExploitType => _resourceSO.ExploitType;

    public bool ExploitByHand => _resourceSO.ExploitByHand;

    #endregion

    private void Awake()
    {
        _Health = _resourceSO.MaxHealth;
        _TimeCount = _resourceSO.TimeCount;
    }

    protected virtual void Start()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        _animation = GetComponent<Animation>();

        audioSource = GetComponent<AudioSource>();

        DestroyResource();
    }

    private void Update()
    {
        if (_Health <= 0 && isCounting == false)
        {
            isCounting = true;

            OutOfResource();
        }
    }

    #region resource display handler

    protected IEnumerator AutoDestroyResource()
    {
        while (_TimeCount > 0)
        {
            _TimeCount -= 1;
            yield return new WaitForSeconds(1);
        }

        isCounting = false;

        DestroyObj();
    }

    private void DestroyObj()
    {
        if(_displayGroup != null)
        {
            Destroy(_displayGroup);
        }

        Destroy(this.gameObject);
    }

    private void DestroyResource()
    {
        if(_IsCanRecovery)
        {
            return;
        }else
        {
            StopAllCoroutines();
            StartCoroutine(AutoDestroyResource());
        }
    }

    public void OutOfResource()
    {
        if(_IsCanRecovery)
        {
            _obstacle.enabled = false;
            this.GetComponent<Collider>().enabled = false;
            _animation.Play();

            StartCoroutine(RecoveryResources());
        }else if(!_IsCanRecovery)
        {
            StopAllCoroutines();
            DestroyObj();
        }
    }

    private void HideDisplay()
    {
        _resourceDisplay.SetActive(false);
    }

    private  IEnumerator RecoveryResources()
    {
        while (_TimeCount > 0)
        {
            _TimeCount -= 1;
            yield return new WaitForSeconds(1);
        }


        _Health = _resourceSO.MaxHealth;
        _obstacle.enabled = true;
        this.GetComponent<Collider>().enabled = true;
        this.transform.localScale = new Vector3(1,1,1);
        _resourceDisplay.SetActive(true);

        _TimeCount = _resourceSO.TimeCount;

        isCounting = false;
    }

    #endregion

    protected void TakeDame(float amount)
    {
        _Health -= amount;
    }

    private MaterialAmount GetRandomItem(List<MaterialAmount> materials)
    {
        if (materials == null || materials.Count == 0) return null;

        // Tính tổng trọng số
        float totalRate = 0;

        foreach (MaterialAmount material in materials)
        {
            totalRate += material._Rate;
        }

        // Sinh số ngẫu nhiên từ 0 đến totalRate
        float randomValue = Random.Range(0, totalRate);

        // Duyệt danh sách để tìm phần tử
        float cumulativeRate = 0;
        foreach (MaterialAmount material in materials)
        {
            cumulativeRate += material._Rate;

            if (randomValue <= cumulativeRate)
            {
                return material;
            }
        }

        return null; // Tránh lỗi, nhưng thường không xảy ra
    }

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    public virtual void Exploited(PlayerController source)
    {
        ToolSO handeldItem = null;

        if (source._CurrentItem != null)
        {
            handeldItem = source._CurrentItem._Item as ToolSO;
        }

        if (handeldItem != null)
        {
            GetItemInResource(source, handeldItem.ToolTag);
        }
        else if (handeldItem == null && _resourceSO.ExploitByHand == true)
        {
            GetItemInResource(source, EToolType.None);
        }

        this.audioSource.PlayOneShot(this.audioClips[RandomSystem.RandomInt(this.audioClips.Count -1, 0)]);

        TakeDame(source._creatureSO.Damage);

    }

    private void GetItemInResource(PlayerController source, EToolType toolType)
    {
        if (_resourceSO.RequiredTools_1.Contains(toolType))
        {
            MaterialAmount rs_1 = GetRandomItem(_resourceSO.Materials_1);

            source._inventory.AddItem(rs_1._Material, rs_1._Amount);

            return;
        }
        else if (_resourceSO.RequiredTools_2.Contains(toolType))
        {
            MaterialAmount rs_2 = GetRandomItem(_resourceSO.Materials_2);

            source._inventory.AddItem(rs_2._Material, rs_2._Amount);

            return;
        }
        else if (_resourceSO.RequiredTools_3.Contains(toolType))
        {
            MaterialAmount rs_3 = GetRandomItem(_resourceSO.Materials_3);

            source._inventory.AddItem(rs_3._Material, rs_3._Amount);

            return;
        }
        else if(_resourceSO.ExploitByHand == true && toolType != EToolType.None)
        {
            GetItemInResource(source, EToolType.None);
            return;
        }


        Debug.LogError(_resourceSO.ResourceName.ToString() + " : resource empty");
    }

    //public void Exploited(PlayerController source)
    //{
    //    ToolSO handeldItem = source._CurrentItem as ToolSO;

    //    MaterialAmount RsMain = _resourceSO.MainMaterials[GetRandomResource(_resourceSO.MainMaterials)];
    //    if (RsMain == null)
    //    {
    //        Debug.LogError("Main materials is empty");
    //    }
    //    MaterialAmount RsSecond = null;
    //    if (_resourceSO.SecondaryMaterials != null && _resourceSO.SecondaryMaterials.Count > 0)
    //    {
    //        RsSecond = _resourceSO.SecondaryMaterials[GetRandomResource(_resourceSO.SecondaryMaterials)];
    //    }

    //    if (handeldItem != null)
    //    {
    //        if(handeldItem.ToolTag == _resourceSO.BestTool)
    //        {
    //            source._inventoryManager.AddItem(RsMain._Material, RsMain._Amount);
    //        }
    //        else if(_resourceSO.RequiredTools.Contains(handeldItem.ToolTag))
    //        {
    //            source._inventoryManager.AddItem(RsSecond._Material, RsSecond._Amount);
    //        }
    //        else if(_resourceSO.RequiredTools == null)
    //        {
    //            source._inventoryManager.AddItem(RsSecond._Material, RsSecond._Amount);
    //        }
    //    }
    //    else if ((handeldItem == null) && _resourceSO.ExploitByHand == true && RsSecond != null)
    //    {
    //        source._inventoryManager.AddItem(RsSecond._Material, RsSecond._Amount);
    //    }
    //    if (handeldItem == null && _resourceSO.ExploitByHand == true && RsSecond == null)
    //    {
    //        source._inventoryManager.AddItem(RsMain._Material, RsMain._Amount);
    //    }

    //    TakeDame(source._creatureSO.Damage);
    //}

}