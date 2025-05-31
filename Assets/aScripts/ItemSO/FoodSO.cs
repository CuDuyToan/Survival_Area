using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Food ()", menuName = "Scriptable/Items/Food")]
public class FoodSO : ItemSO
{
    [Header("Index")]
    [SerializeField] private float foodPoint = 0;
    public float FoodPoint => foodPoint;

    [SerializeField] private float waterPoint = 0;
    public float WaterPoint => waterPoint;

    [SerializeField] private float healthPoint = 0;
    public float HealthPoint => healthPoint;

    [SerializeField]
    private ETimeScale timeScale = ETimeScale.Minute;
    public ETimeScale TimeScale => timeScale;

    [SerializeField, Tooltip("time")]
    private float maxExpiry;
    public float MaxExpiry
    {
        get
        {
            switch (timeScale)
            {
                case ETimeScale.Hour:
                    return maxExpiry * 3600f;
                case ETimeScale.Minute:
                    return maxExpiry * 60f;
                case ETimeScale.Second:
                default:
                    return maxExpiry;
            }
        }
    }
}
