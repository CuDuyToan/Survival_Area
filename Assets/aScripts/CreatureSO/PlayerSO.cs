using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Scriptable/Creatures/Player")]
public class PlayerSO : CreatureSO
{
    [Header("index")]
    [SerializeField, Min(100)] private float maxWater = 100;
    public float MaxWater => maxWater;

    [SerializeField, Tooltip("decrease water point every second")] private float waterDecrease;
    [HideInInspector] public float WaterDecrease => waterDecrease;

    [Header("Player Movement")]
    [SerializeField] private float sprintSpeed;
    [HideInInspector] public float SprintSpeed => sprintSpeed;
}
