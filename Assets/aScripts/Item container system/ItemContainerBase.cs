using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

public class ItemContainerBase : MonoBehaviour
{
    public float TotalWeight
    {
        get
        {
            float totalWeight = 0f;

            foreach (ItemStack item in itemList)
            {
                totalWeight += item.weight;
            }

            return totalWeight;
        }
    }

    #region item list

    [SerializeField] private List<ItemStack> itemList = new List<ItemStack>();
    public List<ItemStack> ItemList => itemList;

    #region add

    public virtual void AddNewItemSlot(ItemStack newItem)
    {
        ItemList.Add(newItem);
    }

    public virtual void AddItem(ItemSO item, int quantity)
    {
        foreach (ItemStack itemInList in ItemList)
        {
            if (quantity <= 0) break;

            if (itemInList._Item == item &&
               itemInList._Quantity < itemInList._Item.MaxStack &&
               quantity > 0)
            {

                itemInList._Quantity += quantity;
                quantity = CalculatorRemainingQuantity(quantity, itemInList);
            }
        }

        if (quantity > 0)
        {
            ItemStack newItem = new ItemStack(item, quantity);

            newItem._Durability = newItem._maxDurability;

            AddNewItemSlot(newItem);
        }
    }

    public virtual void AddItemFromDrop(ItemStack newItem)
    {
        if (newItem == null) return;

        foreach (ItemStack itemInList in ItemList)
        {
            if (newItem._Quantity <= 0) break;

            if (itemInList._Item == newItem._Item &&
               itemInList._Quantity < itemInList._Item.MaxStack &&
               newItem._Quantity > 0)
            {
                int result = newItem._Quantity + itemInList._Quantity;

                newItem._Quantity = CalculatorRemainingQuantity(newItem._Quantity, itemInList);
                itemInList._Quantity = result;

            }
        }

        if (newItem._Quantity > 0)
        {
            ItemList.Add(newItem);
        }
    }
    #endregion add

    #region edit

    public virtual void ConsumeItem(ItemSO itemConsume, int quantity)
    {
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            if (ItemList[i]._Item == itemConsume && quantity > 0)
            {
                int residual = Mathf.Max(0, ItemList[i]._Quantity - quantity);

                ItemList[i]._Quantity -= quantity;

                quantity = residual;
            }
        }
    }

    public void ConsumeThisItem(ItemStack item)
    {
        item._Quantity--;
    }


    #endregion edit

    #region delete

    public void RemoveItem(ItemStack item)
    {
        if (ItemList.Contains(item)) ItemList.Remove(item);
    }


    #endregion delete

    #region search

    public bool ThisItemInList(ItemStack item)
    {
        return ItemList.Contains(item);
    }

    public bool CanBeAddedToSlot(ItemSO item, int quantity)
    {
        foreach (ItemStack member in ItemList)
        {
            if(member._Item == item &&
                quantity <= member._Item.MaxStack - member._Quantity) return true;
        }

        return false;
    }

    public int TotalItemInList(ItemSO item)
    {
        int count = 0;
        foreach (ItemStack member in ItemList)
        {
            if (member._Item == item)
            {
                count += member._Quantity;
            }
        }

        return count;
    }
    public ItemStack GetItemInList(int num)
    {
        return ItemList[num];
    }


    #endregion search

    #region sort

    #endregion sort


    public int ItemListCount()
    {
        return ItemList.Count;
    }

    public void LoadItemData(List<ItemStack> item)
    {
        ItemList.Clear();
        ItemList.AddRange(item); //accept
    }

    #endregion

    protected virtual void Update()
    {
        RefreshItemList();


        ItemRotten();
    }

    private void ItemRotten()
    {
        foreach (ItemStack item in ItemList)
        {
            item.ItemRotten(Time.deltaTime);
        }
    }

    public virtual void RefreshItemList()
    {
        List<ItemStack> itemsToRemove = new List<ItemStack>();

        foreach (ItemStack item in ItemList)
        {
            if (item._Quantity <= 0)
                itemsToRemove.Add(item);
        }

        foreach (ItemStack item in itemsToRemove)
        {
            ItemList.Remove(item);
        }
    }

    protected int CalculatorRemainingQuantity(int quantity, ItemStack item)
    {
        int quantityAvailabe = item._Quantity;
        int maxValue = item._Item.MaxStack;

        if (quantityAvailabe + quantity > maxValue)
            return (quantityAvailabe + quantity) - maxValue;

        return 0;
    }
}