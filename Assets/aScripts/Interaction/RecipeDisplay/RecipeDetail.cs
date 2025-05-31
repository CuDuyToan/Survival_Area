using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDetail : MonoBehaviour
{
    [Header("List item amount")]
    [SerializeField] private ItemAmountDisplay[] _itemAmountGoList;

    public void SetupItemAmountUI(RecipeSO recipe, CraftingBase crafting)
    {
        for (int i = 0; i < recipe.InputItems.Count; i++)
        {
            if (recipe.InputItems[i] == null) continue;

            _itemAmountGoList[i].Setup(crafting, recipe, recipe.InputItems[i]);

            if (i >= 4) break;
        }
    }

    public void ResetItemAmountUI()
    {
        for (int i = 0; i < _itemAmountGoList.Length; i++)
        {
            _itemAmountGoList[i].ResetUI();
        }

    }
}
