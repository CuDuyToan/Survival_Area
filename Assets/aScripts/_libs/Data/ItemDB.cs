using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemDB", menuName = "Data base/Item")]
public class ItemDB : ScriptableObject
{
    [SerializeField] private List<FoodSO> foodList;

    [SerializeField] private List<MaterialSO> materialList;

    [SerializeField] private List<StructureSO> structureList;

    [SerializeField] private List<ToolSO> toolList;

    [SerializeField] private List<WeaponSO> weaponList;
    public List<ItemSO> ItemList
    {
        get
        {
            List<ItemSO> itemList = new List<ItemSO>();

            itemList.AddRange(foodList);
            itemList.AddRange(materialList);
            itemList.AddRange(structureList);
            itemList.AddRange(toolList);
            itemList.AddRange(weaponList);

            return itemList;
        }
    }
}
