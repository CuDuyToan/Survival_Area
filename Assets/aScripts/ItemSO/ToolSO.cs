using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EToolType
{
    None,
    Axe,
    Pickaxe,
    Sickle
}

[CreateAssetMenu(fileName = "Tool ()", menuName = "Scriptable/Items/Tool")]
public class ToolSO : ItemSO
{
    public override bool Stackable => false;

    [SerializeField] private GameObject toolPrefab;
    public GameObject ToolPrefab => toolPrefab;

    [SerializeField] private EToolType toolTag;
    public EToolType ToolTag => toolTag;

    [SerializeField, Min(1)] private int levelTool;

    public int LevelTool => levelTool;

    [SerializeField, Min(100)] private float maxDurability = 100;
    public float MaxDurrability => maxDurability;

    [SerializeField, Min(0)] private float currentDurability;

    public float CurrentDurability
    {
        set { if (value > maxDurability) currentDurability = maxDurability; }
        get { return currentDurability; }
    }

    [SerializeField, Min(100)] private float efficiency; //  (100/100)
    public float Efficiency
    {
        set
        {
            if (value < 100) efficiency = 100;
        }
        get { return efficiency; }
    }

    [SerializeField, Min(1)] private float bonusDame = 1;
    public float BonusDame
    {
        get
        {
            return bonusDame;
        }
    }
}
