using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new StructureDB", menuName = "Data base/Structure")]
public class StructureDB : ScriptableObject
{
    [SerializeField] private List<GameObject> structuresList;
    public List<GameObject> StructureList => structuresList;
}
