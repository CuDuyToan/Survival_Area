using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Baking ()", menuName = "Scriptable/Baking/Recipe")]
public class RecipeFurnaceSO : ScriptableObject
{
    #region input
    [SerializeField] private ItemSO inputItem;
    public ItemSO InputItem => inputItem;

    [SerializeField, Min(1)] private int inputQuantity =1;
    public int InputQuantity => inputQuantity;
    #endregion input


    #region output
    [SerializeField] private ItemSO outputItem;
    public ItemSO OutputItem => outputItem;

    [SerializeField, Min(1)] private int outputQuantity = 1;
    public int OutputQuantity => outputQuantity;
    #endregion output
}