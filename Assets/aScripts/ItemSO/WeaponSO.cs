using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon ()", menuName = "Scriptable/Items/Weapon")]
public class WeaponSO : ItemSO
{
    public override bool Stackable => false;

    [SerializeField] private GameObject weaponPrefab;
    public GameObject WeaponPrefab => weaponPrefab;

    [SerializeField, Min(100)] private float maxDurability = 100;
    public float MaxDurrability => maxDurability;

    [SerializeField, Min(1)] private float bonusDame = 1;
    public float BonusDame
    {
        get
        {
            return bonusDame;
        }
    }
}
