using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item ()", menuName = "Scriptable/Items/Item")]
public class ItemSO : ScriptableObject
{
    //[SerializeField] private int itemID;
    //public int ItemID => itemID;

    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [SerializeField] private Sprite itemSprite;
    public Sprite ItemSprite => itemSprite;

    [SerializeField] private bool stackable;
    public virtual bool Stackable => stackable;

    [SerializeField, Range(1, 1000)] private int maxStack = 1;
    public int MaxStack
    {
        set { if (stackable) maxStack = value;
            else if (!stackable) maxStack = 1;
        }
        get {  return maxStack;}
    }

    [SerializeField, Tooltip("Kg"), Min(0.001f)] private float weight = 0.001f;
    public float Weight => weight;

    [SerializeField, TextArea] private string itemDescription;
    public string ItemDescription => itemDescription;
}


