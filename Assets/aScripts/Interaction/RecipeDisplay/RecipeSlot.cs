using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public RecipeSO _recipe;
    public Image _image;
    public Text _quantity;

    [Header("working station")]
    private CraftingBase _craftingBase;
    private RecipeDetail _recipeDetail;

    [Header("UI")]
    [SerializeField] private Color feasibleColor;
    [SerializeField] private Color notFeasibleColor;

    private void Update()
    {
        UpdateColor();

    }

    public void Setup(RecipeSO recipe, CraftingBase craftingBase, RecipeDetail recipeDetail)
    {
        _craftingBase = craftingBase;
        _recipeDetail = recipeDetail;

        _recipe = recipe;
        _image.sprite = recipe.OutputItem.ItemSprite;
        _quantity.text = recipe.OutputAmount.ToString();

        UpdateColor();
    }

    public void UpdateColor()
    {
        if (_craftingBase.ThisRecipeIsFeasible(_recipe))
        {
            _image.color = feasibleColor; // Đủ đồ → đổi màu xanh
        }
        else
        {
            _image.color = notFeasibleColor; // Không đủ → màu mặc định
        }
    }

    public void OnClickRecipeSlot()
    {
        _recipeDetail.ResetItemAmountUI();

        if (_craftingBase.ThisCurrentRecipe(_recipe))
        {
            _craftingBase.UnSelectRecipe(_recipe);
            return;
        }


        if (_recipe == null) return;

        _craftingBase.SelectRecipe(_recipe);
        _recipeDetail.SetupItemAmountUI(_recipe, _craftingBase);
    }
}
