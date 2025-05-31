using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public string name_SO;

    public int quantity;

    public float durability = 1;

    public ItemData(ItemStack item)
    {
        this.name_SO = item.ItemName;

        this.quantity = item._Quantity;

        if(item._Quantity > 0)
        {
            this.durability = item._Durability;
        }
    }
    public ItemStack ConvertDataToItem()
    {
        ItemDB itemDB = Resources.Load<ItemDB>("ItemDB");

        ItemSO itemSO = itemDB.ItemList.Find(member => member.ItemName == name_SO);

        ItemStack itemStack = new ItemStack(itemSO, quantity);

        if (durability > 0)
        {
            itemStack._Durability = durability;
        }

        return itemStack;
    }
}
