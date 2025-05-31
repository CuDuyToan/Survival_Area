using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;


[CreateAssetMenu(fileName = "Material ()", menuName = "Scriptable/Items/Marterial")]
public class MaterialSO : ItemSO
{
    [Header("fuel")]
    [SerializeField] private bool isFuel;
    public bool IsFuel => isFuel;

    [SerializeField] private float burnTime;
    [SerializeField] private ETimeScale timeScale = ETimeScale.Second;
    public float BurnTime
    {
        get
        {
            CaculatorTime scale = new CaculatorTime(timeScale);

            return burnTime * scale.timeScale;
        }
    }
}
