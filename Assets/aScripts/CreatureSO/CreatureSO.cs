using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ECreatureType
//{
//    Passive,
//    Enemy,
//    Neutral,
//    Player
//}

[CreateAssetMenu(fileName = "NewCreature", menuName = "Scriptable/Creatures/Creature")]
public class CreatureSO : ScriptableObject
{
    [SerializeField] private string creatureName;
    public string Name => creatureName;

    //[SerializeField] private ECreatureType creatureType;
    //[HideInInspector] public ECreatureType CreatureType => creatureType;

    [SerializeField, Min(100)] private float weightCapacity;
    public float WeightCapacity => weightCapacity;

    [Header("Health")]
    [SerializeField, Min(1)] private float maxHealth = 100;
    public float MaxHealth => maxHealth;

    [Header("Food")]
    [SerializeField, Min(100)] private float maxFood = 100;
    public float MaxFood => maxFood;

    [SerializeField, Tooltip("decrease food point every second"), Min(0.1f)] private float foodDecrease = 0.1f;
    public float FoodDecrease => foodDecrease;

    [Header("Stamina"), Min(100)]
    [SerializeField] private float maxStamina = 100;
    public float MaxStamina => maxStamina;

    [Header("Recovery"), Min(0.1f)]
    [SerializeField] private float recoveryValue = 0.1f;
    public float RecoveryValue => recoveryValue;

    [SerializeField, Tooltip("recovery health by % max index"), Min(0.1f)] private float recoveryRate = 0.1f;
    public float RecoveryRate => recoveryRate;

    [Header("Combat Index")]
    [SerializeField, Min(0)] private float damage = 5;
    public float Damage => damage;

    [SerializeField, Min(0)] private float reduceDamage = 1;
    public float ReduceDamage => reduceDamage;

    [SerializeField, Min(0)] private float armor = 1;
    public float Armor => armor;

    [Header("Movement")]
    [SerializeField, Min(1)] private float walkSpeed = 1;
    public float WalkSpeed => walkSpeed;

    [SerializeField, Min(2)] private float runSpeed = 2;
    public float RunSpeed => runSpeed;
}
