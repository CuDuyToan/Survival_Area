using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGroupingType
{
    PackBased,
    SoloBased
}

[CreateAssetMenu(fileName = "Danger Creature ", menuName = "Scriptable/Creatures/Danger")]
public class DangerCreatureSO : CreatureSO
{
    [Header("Enemy Index")]
    [SerializeField] private EGroupingType groupType;
    [HideInInspector] public EGroupingType GroupingType => groupType;

    [SerializeField] private float fieldOfViewAngle;
    [HideInInspector] public float FieldOfViewAngle => fieldOfViewAngle;

    [SerializeField] private LayerMask creatureLayer;
    [HideInInspector] public LayerMask CreatureLayer => creatureLayer;

    //[SerializeField] private List<string> listNamePrey;
    //[HideInInspector] public List<string> ListNamePrey => listNamePrey;

}
