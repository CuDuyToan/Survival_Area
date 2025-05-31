using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAmountDisplay : MonoBehaviour
{
    public Image _image;
    public Text _quantity;

    private RecipeSO _recipe;
    private ItemAmount _itemAmount;
    private CraftingBase _craftingBase;

    [Header("UI")]
    [SerializeField] private Color feasibleColor;
    [SerializeField] private Color notFeasibleColor;

    private void Update()
    {
        RefreshUI();
    }

    public void Setup(CraftingBase craftingBase, RecipeSO recipe, ItemAmount itemAmount)
    {
        _craftingBase = craftingBase;
        _recipe = recipe;
        _itemAmount = itemAmount;

        _image.sprite = _itemAmount.item.ItemSprite;
    }

    public void ResetUI()
    {
        _recipe = null;
        _image.sprite = null;
        _itemAmount = null;
        _quantity.text = "";
    }

    private void RefreshUI()
    {
        if (_recipe == null || _craftingBase == null)
        {
            _quantity.text = "";
            return;
        }
        else
        {
            _quantity.text = _craftingBase.QuantityDifference(_itemAmount.item, _itemAmount._Amount);
        }

        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_craftingBase.ThisItemEnough(_itemAmount.item, _itemAmount._Amount)) _quantity.color = feasibleColor;
        else _quantity.color = notFeasibleColor;
    }
}
