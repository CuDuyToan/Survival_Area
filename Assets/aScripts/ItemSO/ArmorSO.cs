using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Armor ()", menuName = "Scriptable/Items/Armor")]
public class ArmorSO : ItemSO
{
    [SerializeField] private EArmorPosition armorPosition;
    public EArmorPosition ArmorPosition => armorPosition;

    [SerializeField, Min(0)] private float durability;
    public float Durability => durability;
}

public enum EArmorPosition
{
    head,
    hand,
    body,
    foot
}
