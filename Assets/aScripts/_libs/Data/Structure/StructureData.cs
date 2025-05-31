using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureData
{
    public string nameStructure;

    public float health;

    public PositionData position;

    public RotationData rotation;

    public List<ItemData> itemStorage = new List<ItemData>();

    public StructureData(Structure structure, Transform structureTransform)
    {
        this.nameStructure = structure._StructureSO.ItemName;
        this.health = structure._Health;

        this.position = new PositionData(structureTransform.position);
        this.rotation = new RotationData(structureTransform.eulerAngles);

        //this.name = name;
        //this.health = health;

        //this.position = position;
        //this.rotation = rotation;


        //Debug.Log($" pos : {position.x} {position.y} {position.z} rotation : {rotation.x} {rotation.y} {rotation.z}");
    }

    public void SaveItemStorage(List<ItemStack> itemList)
    {
        foreach (ItemStack item in itemList)
        {
            //ItemData itemData = new ItemData(item.ItemName, item._Quantity, item._Durability);

            ItemData itemData = new ItemData(item);

            itemStorage.Add(itemData);
        }
    }

}
