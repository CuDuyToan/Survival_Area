using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EExploitType
{
    Gather,
    Mine
}

public enum ETimeScale
{
    Hour,
    Minute,
    Second,
}

public class CaculatorTime
{
    public float timeScale = 1;

    public CaculatorTime(ETimeScale type)
    {
        switch (type)
        {
            case ETimeScale.Hour:
                timeScale = timeScale * 3600f;
                break;
            case ETimeScale.Minute:
                timeScale = timeScale * 60f;
                break;
            case ETimeScale.Second:
            default:
                timeScale = timeScale * 1;
                break;
        }
    }
}

[CreateAssetMenu(fileName = "Resource ()", menuName = "Scriptable/Resources/Resource")]

public class ResourceSO : ScriptableObject
{
    [SerializeField] private string resourceName;
    public string ResourceName => resourceName;

    [SerializeField] EExploitType exploitType;
    public EExploitType ExploitType => exploitType;

    [SerializeField] private bool exploitByHand;
    public bool ExploitByHand => exploitByHand;

    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    [Header("Recovery")]
    [SerializeField] private bool isCanRecovery = true;
    public bool IsCanRecovery => isCanRecovery;

    [SerializeField, Tooltip("Time for resources to recover after being destroyed (Time Scale)")]
    private ETimeScale timeScale;
    public ETimeScale TimeScale => timeScale;

    [SerializeField, Tooltip("If it can be recovered then this will be the recovery time, otherwise it will be the deleted time")] 
    private float timeCount;
    public float TimeCount
    {
        get {

            CaculatorTime scale = new CaculatorTime(TimeScale);

            return timeCount * scale.timeScale;

            //switch (timeScale)
            //{
            //    case ETimeScale.Hour:
            //        return timeCount * 3600f;
            //    case ETimeScale.Minute:
            //        return timeCount * 60f;
            //    case ETimeScale.Second:
            //    default:
            //        return timeCount;
            //}
        }
    }

    [Header("Material type 1")]
    [SerializeField] private List<EToolType> requiredTools_1;
    public List<EToolType> RequiredTools_1 => requiredTools_1;

    [SerializeField] private List<MaterialAmount> materials_1;
    public List<MaterialAmount> Materials_1 => materials_1;

    [Header("Material type 2")]
    [SerializeField] private List<EToolType> requiredTools_2;
    public List<EToolType> RequiredTools_2 => requiredTools_2;

    [SerializeField] private List<MaterialAmount> materials_2;
    public List<MaterialAmount> Materials_2 => materials_2;


    [Header("Material type 3")]
    [SerializeField] private List<EToolType> requiredTools_3;
    public List<EToolType> RequiredTools_3 => requiredTools_3;

    [SerializeField] private List<MaterialAmount> materials_3;
    public List<MaterialAmount> Materials_3 => materials_3;
}

[System.Serializable]
public class MaterialAmount
{
    [SerializeField] private ItemSO _material;

    public ItemSO _Material => _material;

    [SerializeField, Min(1)] private int _amount = 1;

    public int _Amount
    {
        get 
        {
            if (_amount <= 0) return 1;
            return _amount;
        }
    }

    [SerializeField, Min(1)] private float _rate = 1;

    public float _Rate => _rate;
}
