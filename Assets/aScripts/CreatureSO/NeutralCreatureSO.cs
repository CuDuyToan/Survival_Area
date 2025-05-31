using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Neutral Creature ", menuName = "Scriptable/Creatures/Neutral")]
public class NeutralCreatureSO : CreatureSO
{
    [SerializeField] private float fieldOfViewAngle;
    public float FieldOfViewAngle => fieldOfViewAngle;
}
