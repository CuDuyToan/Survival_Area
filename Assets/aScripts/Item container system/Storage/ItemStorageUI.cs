using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemStorageUI : ItemContainUI
{
    public override void OnDrop(PointerEventData eventData)
    {
        ItemDisplayUI itemDisplay = eventData.pointerDrag.GetComponent<ItemDisplayUI>();

        if (ItemContainer is ItemStorage storage && storage.ItemListCount() >= storage.MaxSlot)
        {
            if (!storage.CanBeAddedToSlot(itemDisplay._itemStack._Item, itemDisplay._itemStack._Quantity)) return;
        }

        base.OnDrop(eventData);
    }


    private void OnEnable()
    {
        LoadStorageUI();

        if (ItemContainer is ItemStorage storage)
        {
            storage.storageUI = this;
        }
    }

    private void OnDisable()
    {
        RemoveAllSlot();
        
        if(ItemContainer is ItemStorage storage)
        {
            storage.storageUI = null;
        }
    }

    private void LoadStorageUI()
    {
        if (ItemContainer is ItemStorage storage)
        {
            for (int i = 0; i < storage.ItemListCount(); i++)
            {
                if (/*i < storage.MaxSlot*/ true)
                {
                    ItemStack itemStack = storage.GetItemInList(i);

                    SpawnNewItemDisplay(itemStack, _contentTransform.transform);
                }
            }
        }
    }

    public void UpdateStorageUI(ItemStack newItem)
    {
        SpawnNewItemDisplay(newItem, _contentTransform.transform);
    }

    private void RemoveAllSlot()
    {
        for (int i = 0; i < _contentTransform.transform.childCount; i++)
        {
            Transform child = _contentTransform.transform.GetChild(i);

            if (child.gameObject) Destroy(child.gameObject);
        }
    }
}
