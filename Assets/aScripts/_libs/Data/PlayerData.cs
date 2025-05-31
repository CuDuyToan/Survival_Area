using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float food;
    public float stamina;
    public float water;

    public PositionData position;
    public RotationData rotation;

    public List<ItemData> inventoryData = new List<ItemData>();

    public PlayerData(PlayerController player, Transform transform, InventoryPlayer inventory)
    {
        this.health = player._Health;
        this.food = player._Food;
        this.stamina = player._Stamina;
        this.water = player.Water;

        position = new PositionData(transform.position);
        rotation = new RotationData(transform.eulerAngles);

        this.ConvertItemToData(inventory.ItemList);
    }

    private void ConvertItemToData(List<ItemStack> itemList)
    {
        foreach (ItemStack item in itemList)
        {
            ItemData itemData = new ItemData(item);

            inventoryData.Add(itemData);
        }
    }
}
