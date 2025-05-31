using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Scriptable/Crafting/Recipe")]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private ItemSO outputItem;
    public ItemSO OutputItem => outputItem;

    [SerializeField] private float creationTime = 0;
    public float CreationTime => creationTime;

    //[SerializeField] private EIntermediaryTool intermediaryTool;
    //public EIntermediaryTool IntermediaryTool => intermediaryTool;


    [SerializeField] private List<ItemAmount> inputItems;
    public List<ItemAmount> InputItems
    {
        get
        {
            if(inputItems.Count > 4)
            {
                for (int i = 0; i < inputItems.Count; i++)
                {
                    if (i > 4) inputItems.RemoveAt(i);
                }
            }

            return inputItems;
        }
    }

    [SerializeField, Min(1)] private int outputAmount;
    public int OutputAmount
    {
        get
        {
            if (outputAmount <= 0) outputAmount = 1;
            return outputAmount;
        }
    }
}

[System.Serializable]
public class ItemAmount
{
    public ItemSO item;

    [SerializeField] private int _amount;

    public int _Amount
    {
        set 
        {
            _amount = value; 
        }
        get 
        {
            return _amount;
        }
    }
}

