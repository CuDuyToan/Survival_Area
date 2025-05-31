using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject itemBox;

    [SerializeField] private Transform itemBoxGroup;

    [SerializeField] private GameObject player;

    public void OnDrop(PointerEventData eventData)
    {
        ItemDisplayUI itemDisplay = eventData.pointerDrag.GetComponent<ItemDisplayUI>();
        if (itemDisplay == null) return;

        Drop(itemDisplay);
    }

    private void Drop(ItemDisplayUI itemDisplay)
    {
        if (itemDisplay._parentAfterDrag == null) return;

        ItemContainLink link = itemDisplay._parentAfterDrag.GetComponent<ItemContainLink>();

        if (link == null) return;

        if(link.InventoryContainer.ItemList.Contains(itemDisplay._itemStack))
        {
            link.InventoryContainer.ItemList.Remove(itemDisplay._itemStack);
        }

        Destroy(itemDisplay.gameObject);

        GameObject Box = Instantiate(this.itemBox, this.itemBoxGroup);

        Box.SetActive(false);

        ItemBox itemBox = Box.GetComponent<ItemBox>();

        itemBox.Item = itemDisplay._itemStack._Item;
        itemBox.Quantity = itemDisplay._itemStack._Quantity;

        Vector3 pos = player.transform.position;

        pos.z += 1;
        pos.y += 1;

        Box.transform.position = pos;

        Box.SetActive(true);

        Rigidbody rb = Box.GetComponent<Rigidbody>();

        rb.AddForce(player.transform.forward * 2f, ForceMode.Impulse);
    }
}
