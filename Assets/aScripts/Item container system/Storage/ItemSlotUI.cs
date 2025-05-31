using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : ItemContainUI
{
    public override void OnDrop(PointerEventData eventData)
    {
        ItemDisplayUI itemDisplay = eventData.pointerDrag.GetComponent<ItemDisplayUI>();
        if (itemDisplay == null) return;

        SwapStorage(itemDisplay, ItemContainer);

        SwapSlotUI(itemDisplay);
    }

    protected override void SwapSlotUI(ItemDisplayUI itemDrag)
    {
        if(this.transform.childCount > 0)
        {
            ItemDisplayUI thisChild = this.transform.GetChild(0).gameObject.GetComponent<ItemDisplayUI>();
            if (thisChild == null) return;

            thisChild._parentAfterDrag = itemDrag._parentAfterDrag;
            thisChild.transform.SetParent(thisChild._parentAfterDrag);
        }

        base.SwapSlotUI(itemDrag);
    }
}
