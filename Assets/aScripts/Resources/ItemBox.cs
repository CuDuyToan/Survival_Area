using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Resource
{
    private ItemSO item;
    public ItemSO Item
    {
        set
        {
            this.item = value;
        }
        get
        {
            return this.item;
        }
    }

    private int quantity = 1;

    public int Quantity
    {
        set
        {
            this.quantity = value;
        }
        get
        {
            return this.quantity;
        }
    }

    public override void Exploited(PlayerController source)
    {
        source._inventory.AddItem(this.Item, this.Quantity);

        TakeDame(source._creatureSO.Damage);
    }
}
