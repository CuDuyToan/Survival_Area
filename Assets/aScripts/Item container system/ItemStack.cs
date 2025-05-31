using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public string ItemName => _item.ItemName;

    [SerializeField] private ItemSO _item;
    public ItemSO _Item
    {
        get
        {
            if (_item == null) return null;

            if (_item is FoodSO foodData) _maxDurability = foodData.MaxExpiry;
            else if (_item is ToolSO toolData) _maxDurability = toolData.MaxDurrability;
            else if (_item is WeaponSO weapon) _maxDurability = weapon.MaxDurrability;

            return _item;
        }
    }

    #region index

    [SerializeField] private int _quantity = 1;
    public int _Quantity
    {
        set
        {
            if (!_Item.Stackable && value > 1) _quantity = 1;
            else if (value > _Item.MaxStack) _quantity = _Item.MaxStack;
            else if(value < 0) _quantity = 0;
            else _quantity = value;
        }
        get
        {
            if (_Item == null) return 0;

            if (!_Item.Stackable && _quantity > 1)
            {
                _quantity = 1;
            }
            return _quantity;
        }
    }

    public float _maxDurability = 1;
    [SerializeField] private float _durability = 1;
    public float _Durability
    {
        set
        {
            if (_Item is ToolSO toolData)
            {
                if (value > toolData.MaxDurrability) _durability = toolData.MaxDurrability;
                else if (value < 0) _durability = 0;
                else _durability = value;
            }
            else if (_Item is FoodSO foodData)
            {
                if (value > foodData.MaxExpiry) _durability = foodData.MaxExpiry;
                else if (value < 0) _durability = 0;
                else _durability = value;
            }
            else if (_item is WeaponSO weaponData)
            {
                if(value > weaponData.MaxDurrability) _durability = weaponData.MaxDurrability;
                else if(value < 0) _durability = 0;
                else _durability = value;
            }

            if (_durability <= 0)
            {
                _Quantity -= 1;

                if(_Quantity > 0)
                {
                    _durability = _maxDurability;
                }

            }
        }
        get
        {
            return _durability;
        }
    }
    public float weight
    {
        get
        {
            float itemWeight = (float)Mathf.Round(_Item.Weight * 10000f) / 10000f;
            return itemWeight * _Quantity;
        }
    }

    #endregion

    public ItemStack(ItemSO item, int quantity)
    {
        this._item = item;
        this._Quantity = quantity;
    }

    public void ItemRotten(float rottenValue)
    {
        if (_Item is FoodSO)
        {
            _Durability -= rottenValue;
        }
    }
}