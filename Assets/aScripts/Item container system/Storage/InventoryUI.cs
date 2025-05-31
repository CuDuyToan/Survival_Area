using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : ItemContainUI
{
    private void Awake()
    {
        InventoryPlayer.OnNewItemSlot_UI += NewItem;
    }

    private void NewItem(ItemStack newItem)
    {
        SpawnNewItemDisplay(newItem, _contentTransform);
    }
}
