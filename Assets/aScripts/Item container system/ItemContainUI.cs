using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemContainUI : MonoBehaviour, IDropHandler
{
    [Header("Container")]
    [SerializeField] private ItemContainerBase _itemContainer;
    public ItemContainerBase ItemContainer => _itemContainer;

    public ItemContainerBase SetItemContainer 
    {
        set 
        { 
            _itemContainer = value;
        } 
    }

    [Header("Prefab")]
    [SerializeField] private GameObject _itemDisplay_prefab;

    protected void SpawnNewItemDisplay(ItemStack item, Transform parrentTransform)
    {
        //if (ItemContainer.CanBeAddedToSlot(item._Item, item._Quantity)) return;

        GameObject newItemDisplay = Instantiate(_itemDisplay_prefab, parrentTransform);
        ItemDisplayUI itemDisplayUI = newItemDisplay.GetComponent<ItemDisplayUI>();
        itemDisplayUI._itemStack = item;

        newItemDisplay.name = $"Item [{item._Item.name}]";
    }

    #region item drop

    [Header("item display (content)")]
    [SerializeField, Tooltip("item contain content")]
    protected Transform _contentTransform;
    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemDisplayUI itemDisplay = eventData.pointerDrag.GetComponent<ItemDisplayUI>();
        if (itemDisplay == null) return;

        SwapStorage(itemDisplay, ItemContainer);

        SwapSlotUI(itemDisplay);
    }

    protected void SwapStorage(ItemDisplayUI itemDrag, ItemContainerBase itemContainer)
    {
        ItemContainLink itemDragLinkStorage = itemDrag._parentAfterDrag.GetComponent<ItemContainLink>();

        if (itemDragLinkStorage == null) return;

        ItemContainerBase itemDragStorage = itemDragLinkStorage.InventoryContainer;

        ItemContainerBase thisStorage = itemContainer;

        itemDragStorage.RemoveItem(itemDrag._itemStack);
        thisStorage.AddItemFromDrop(itemDrag._itemStack);
    }

    protected virtual void SwapSlotUI(ItemDisplayUI itemDrag)
    {
        itemDrag._parentAfterDrag = _contentTransform;
    }

    #endregion
}
