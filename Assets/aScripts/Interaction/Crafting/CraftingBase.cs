using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CraftingBase : MonoBehaviour
{
    [Header("inventory player")]
    protected ItemContainerBase _itemstorage = null;

    #region item list

    protected List<ItemStack> _itemList = new List<ItemStack>();

    protected virtual void ConsumeItem(ItemSO itemConsume, int quantity)
    {
        for (int i = _itemstorage.ItemList.Count - 1; i >= 0; i--)
        {
            if (_itemstorage.ItemList[i]._Item == itemConsume && quantity > 0)
            {
                int residual = Mathf.Max(0, quantity - _itemstorage.ItemList[i]._Quantity);

                _itemstorage.ItemList[i]._Quantity -= quantity;

                quantity = residual;
            }
        }
    }

    #endregion item list

    [Header("list recipe")]
    [SerializeField] private List<RecipeSO> _listRecipe;
    public List<RecipeSO> ListRecipe => _listRecipe;

    public bool ThisRecipeInList(RecipeSO recipe)
    {
        return _listRecipe.Contains(recipe);
    }

    #region crafting
    [Header("crafting")]
    [SerializeField] private RecipeSO _currentRecipe;
    public bool ThisCurrentRecipe(RecipeSO recipe)
    {
        return (_currentRecipe == recipe);
    }

    public void SelectRecipe(RecipeSO recipe)
    {
        if (_listRecipe.Contains(recipe)) _currentRecipe = recipe;
    }

    public void UnSelectRecipe(RecipeSO recipe)
    {
        if(recipe == _currentRecipe) _currentRecipe = null;
    }
    #endregion crafting

    protected virtual void Awake()
    {
        _itemstorage = this.GetComponent<ItemContainerBase>();
        //_itemList.AddRange(_itemstorage.ItemList);
    }

    #region check condition

    public virtual string QuantityDifference(ItemSO item, int quantityRequired)
    {
        return $"{_itemstorage.TotalItemInList(item)} / {quantityRequired}";
    }

    public virtual bool ThisItemEnough(ItemSO item, int quantityRequired)
    {
        return _itemstorage.TotalItemInList(item) >= quantityRequired;
    }

    public virtual bool ThisRecipeIsFeasible(RecipeSO recipe)
    {
        if(recipe == null) return false;
        if(_itemstorage == null)
        {
            Debug.Log("item storage is null");
            return false;
        }

        foreach (ItemAmount itemAmount in recipe.InputItems)
        {
            if (_itemstorage.TotalItemInList(itemAmount.item) < itemAmount._Amount) 
            {
                return false;
            }
        }

        return true;
    }

    #endregion check condition

    public void CraftItem()
    {
        if (!ThisRecipeIsFeasible(_currentRecipe)) return;

        foreach (ItemAmount itemAmount in _currentRecipe.InputItems)
        {
            ConsumeItem(itemAmount.item, itemAmount._Amount);
        }

        _itemstorage.AddItem(_currentRecipe.OutputItem, _currentRecipe.OutputAmount);

    }



}
