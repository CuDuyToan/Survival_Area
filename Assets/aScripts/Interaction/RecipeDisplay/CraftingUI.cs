using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    #region private member
    /*[SerializeField]*/ private CraftingBase _craftingBase;
    public CraftingBase SetCraftingBase
    {
        set
        {
            _craftingBase = value;
        }
    }

    [SerializeField] private RecipeDetail _recipeDetail;
    [SerializeField] private GameObject _recipeSlotPrefab;
    [SerializeField] private GameObject _content;

    private List<GameObject> _listRecipeGo = new List<GameObject>();
    #endregion

    private void OnEnable()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        ClearUI();
    }

    private void ClearUI()
    {
        foreach (GameObject recipeObj in _listRecipeGo)
        {
            Destroy(recipeObj);
        }
        _listRecipeGo.Clear();
    }

    private void RefreshUI()
    {
        if(_listRecipeGo != null) _listRecipeGo.Clear();

        //if (_craftingBase == null) return;

        foreach (RecipeSO recipe in _craftingBase.ListRecipe)
        {
            GameObject recipeGo = Instantiate(_recipeSlotPrefab, _content.transform);

            RecipeSlot recipeSlot = recipeGo.GetComponent<RecipeSlot>();

            recipeSlot.Setup(recipe, _craftingBase, _recipeDetail);

            _listRecipeGo.Add(recipeGo);
        }
    }

    #region button
    public void OnClickCraftingButton()
    {
        //if (_craftingBase == null) return;

        _craftingBase.CraftItem();
    }

    #endregion button

}
