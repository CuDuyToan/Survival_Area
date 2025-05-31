using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : CraftingBase
{
    [SerializeField] private InventoryPlayer _inventoryPlayer;

    public InventoryPlayer InventoryPlayer
    {
        set
        {
            _inventoryPlayer = value;
        }
        get
        {
            if(_inventoryPlayer == null)
            {
                _inventoryPlayer = FindObjectOfType<InventoryPlayer>();
            }
            return _inventoryPlayer;
        }
    }

    #region item list
    protected override void ConsumeItem(ItemSO itemConsume, int quantity)
    {
        base.ConsumeItem(itemConsume, quantity);

        for (int i = InventoryPlayer.ItemList.Count - 1; i >= 0; i--)
        {
            if (InventoryPlayer.ItemList[i]._Item == itemConsume && quantity > 0)
            {
                int residual = Mathf.Max(0, InventoryPlayer.ItemList[i]._Quantity - quantity);

                InventoryPlayer.ItemList[i]._Quantity -= quantity;

                quantity = residual;
            }
        }
    }


    #endregion item list


    #region check condition

    public override string QuantityDifference(ItemSO item, int quantityRequired)
    {
        return $"{_itemstorage.TotalItemInList(item) + InventoryPlayer.TotalItemInList(item)} / {quantityRequired}";
    }

    public override bool ThisItemEnough(ItemSO item, int quantityRequired)
    {
        return _itemstorage.TotalItemInList(item) + InventoryPlayer.TotalItemInList(item) >= quantityRequired;
    }

    public override bool ThisRecipeIsFeasible(RecipeSO recipe)
    {
        if (recipe == null) return false;

        foreach (ItemAmount itemAmount in recipe.InputItems)
        {
            if (_itemstorage.TotalItemInList(itemAmount.item) + InventoryPlayer.TotalItemInList(itemAmount.item) < itemAmount._Amount)
            {
                return false;
            }
        }

        return true;
    }

    #endregion check condition
}
