using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPlayer : ItemContainerBase
{
    public static event Action<ItemSO, int> OnReceivedItem_notice;
    public static event Action<ItemStack> OnNewItemSlot_UI;
    public static event Action<ItemSO, int> OnConsumeItem_notice;

    private List<ItemStack> inventory = new List<ItemStack>();

    private ItemStack itemSelect = null;
    public static event Action<ItemStack> OnSelectItem;

    public void SelectItem(ItemStack item)
    {
        if (item == itemSelect)
        {
            item = null;
        }

        OnSelectItem?.Invoke(item);

        itemSelect = item;
    }

    public override void ConsumeItem(ItemSO itemConsume, int quantity)
    {
        OnConsumeItem_notice?.Invoke(itemConsume, quantity);

        base.ConsumeItem(itemConsume, quantity);
    }

    public override void AddItem(ItemSO item, int quantity)
    {
        OnReceivedItem_notice?.Invoke(item, quantity);

        base.AddItem(item, quantity);
    }

    public override void AddNewItemSlot(ItemStack newItem)
    {
        base.AddNewItemSlot(newItem);
        OnNewItemSlot_UI?.Invoke(newItem);
    }
}
