using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CreatureDB", menuName = "Data base/Creature")]
public class CreatureDB : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyList;

    public List<GameObject> creatureList
    {
        get
        {
            List<GameObject> creatureList = new List<GameObject>();

            creatureList.AddRange(enemyList);

            return creatureList;
        }
    }
}
