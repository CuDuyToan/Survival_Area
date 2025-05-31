using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairStructure : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    //[SerializeField] private Image _filled;

    //[SerializeField] private float _waitTime;
    //private float _timeCount;
    //private float _TimeCount
    //{
    //    set
    //    {
    //        if (value > _waitTime) _timeCount = _waitTime;
    //        else _timeCount = value;
    //    }
    //    get
    //    {
    //        return _timeCount;
    //    }
    //}

    private GameObject _structure;


    private void OnEnable()
    {
        InputHandle.OnRepairStructure += PressButton;
        InputHandle.OnRaycastHit += SetStructureObj;
    }

    private void OnDisable()
    {
        InputHandle.OnRepairStructure -= PressButton;
        InputHandle.OnRaycastHit -= SetStructureObj;
    }

    private void SetStructureObj(GameObject gameObject)
    {
        _structure = gameObject;
    }

    private Coroutine waitCoroutine;

    private void PressButton()
    {
        Repair(_structure);
    }

    public void Repair(GameObject target)
    {
        Structure structure = target.GetComponent<Structure>();

        if (structure != null)
        {
            float _healthRate = structure._Health / structure._StructureSO.MaxHealth;

            if(CheckItemAmount(structure._RecipeSO, _healthRate))
            {
                RepairResourceCalculation(structure._RecipeSO, _healthRate);

                structure._Health = structure._StructureSO.MaxHealth;
            }
        }
    }

    private void RepairResourceCalculation(RecipeSO recipeSO, float rate)
    {
        foreach (ItemAmount itemAmount in recipeSO.InputItems)
        {
            int amount = (int)Math.Ceiling(itemAmount._Amount - (itemAmount._Amount * rate));

            if (amount <= 0) continue;


            _player._inventory.ConsumeItem(itemAmount.item, amount);
        }
    }

    public static event Action<ItemSO, int> itemNeedNotice;

    private bool CheckItemAmount(RecipeSO recipeSO, float rate)
    {
        bool state = true;

        foreach (ItemAmount itemAmount in recipeSO.InputItems)
        {
            int amount = (int)Math.Ceiling(itemAmount._Amount - (itemAmount._Amount * rate));
            if (amount <= 0) continue;

            int itemQuantity = _player._inventory.TotalItemInList(itemAmount.item); ;

            if(itemQuantity < amount)
            {
                itemNeedNotice?.Invoke(itemAmount.item , amount - itemQuantity);
                state = false;
            }
        }

        return state;
    }



}
