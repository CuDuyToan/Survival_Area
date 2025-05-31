using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainLink : MonoBehaviour
{
    [SerializeField] private ItemContainUI itemContainUI;
    public ItemContainUI ItemContainUI 
    { 
        set 
        {
            itemContainUI = value;
        }
    }
    public ItemContainerBase InventoryContainer => itemContainUI.ItemContainer;

    private void OnTransformChildrenChanged()
    {
        //Debug.Log("A child was added or removed!");
    }
}
