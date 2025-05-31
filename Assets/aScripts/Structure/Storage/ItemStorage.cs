using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : ItemContainerBase
{
    [SerializeField] private int maxSlot = 1;
    public int MaxSlot => maxSlot;

    //public void CleanItemList()
    //{
    //    if (ItemList.Count <= maxSlot) return;

    //    for (int i = ItemList.Count - 1; i > maxSlot; i--)
    //    {
    //        ItemList.Remove(ItemList[i]);
    //    }
    //}

    [HideInInspector] public ItemStorageUI storageUI;

    public override void AddNewItemSlot(ItemStack newItem)
    {
        base.AddNewItemSlot(newItem);

        if(storageUI)
        {
            storageUI.UpdateStorageUI(newItem);
        }
    }

    public override void AddItemFromDrop(ItemStack newItem)
    {
        if (ItemList.Count >= maxSlot) return;
        base.AddItemFromDrop(newItem);
    }
}
