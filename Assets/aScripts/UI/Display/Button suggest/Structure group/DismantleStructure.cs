using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismantleStructure : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    [SerializeField] private Image _filled;
    [SerializeField] private float _waitTime;
    private float _timeCount;
    private float _TimeCount
    {
        set
        {
            if (value > _waitTime) _timeCount = _waitTime;
            else _timeCount = value;
        }
        get
        {
            return _timeCount;
        }
    }

    private GameObject _structure;


    private void OnEnable()
    {
        StopAllCoroutines();

        _TimeCount = 0;

        InputHandle.OnDismantleStructure += PressButton;
        InputHandle.OnRaycastHit += SetStructureObj;
    }

    private void OnDisable()
    {
        InputHandle.OnDismantleStructure -= PressButton;
        InputHandle.OnRaycastHit -= SetStructureObj;
    }

    private void SetStructureObj(GameObject gameObject)
    {
        _structure = gameObject;
    }

    private Coroutine waitCoroutine;

    private void PressButton(bool state)
    {
        if (state)
        {
            if (waitCoroutine == null) // Đảm bảo không chạy trùng lặp
            {
                waitCoroutine = StartCoroutine(Wait());
            }
        }
        else
        {
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);

                _TimeCount = 0;

                waitCoroutine = null;
            }
        }
    }

    private IEnumerator Wait()
    {
        _TimeCount = 0f;
        while (_TimeCount < _waitTime)
        {
            _TimeCount += Time.deltaTime; 
            yield return null;
        }

        Debug.Log("Đã giữ đủ lâu!"); 
        waitCoroutine = null;
    }

    private void Update()
    {
        _filled.fillAmount = _TimeCount / _waitTime;

        if (_structure && _TimeCount >= _waitTime) Dismantle(_structure);
    }

    public void Dismantle(GameObject target)
    {
        Structure structure = target.GetComponent<Structure>();

        if (structure != null)
        {
            float dismantleRate = structure._Health / structure._StructureSO.MaxHealth;

            DismantlingResourceCalculation(structure._RecipeSO, dismantleRate);
            TakeItemOnStructure(structure);

            Destroy(target);
        }
    }

    private void DismantlingResourceCalculation(RecipeSO recipeSO, float rate)
    {
        foreach (ItemAmount item in recipeSO.InputItems)
        {
            float amount = item._Amount * rate;
            if (amount <= 0) amount = 1;

            _player._inventory.AddItem(item.item, (int)amount);
        }
    }

    private void TakeItemOnStructure(Structure structure)
    {
        ItemContainerBase itemContainer = structure.GetComponent<ItemContainerBase>();
        if(itemContainer != null)
        {
            foreach (ItemStack item in itemContainer.ItemList)
            {
                _player._inventory.AddItem(item._Item, item._Quantity);
            }
        }
    }

}
