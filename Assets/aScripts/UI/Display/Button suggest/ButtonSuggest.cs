using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSuggest : MonoBehaviour
{
    [SerializeField] private float _distance;

    [SerializeField, Header("Player")] private PlayerController _player;

    [Header("button suggest")]
    [SerializeField] private GameObject _button_structureGroup;
    [SerializeField] private GameObject _button_activeStructure;

    private void OnEnable()
    {
        InputHandle.OnRaycastHit += Handle;
    }

    private void OnDisable()
    {
        InputHandle.OnRaycastHit -= Handle;
    }

    private bool CheckDistance(GameObject gameObject)
    {
        float distance = (gameObject.transform.position - _player.transform.position).magnitude;

        if (distance <= _distance)
        {
            return true;
        }
        return false;
    }

    private void Handle(GameObject gameObject)
    {
        if (gameObject == null)
        {
            HideAll();
            return;
        }

        InteractStructure(gameObject);
    }

    #region structure

    private void StructureBuilding()
    {
        //ItemSO currentItem = _player._CurrentItem._Item;

        //if (currentItem == null) return;

        //if (currentItem is StructureSO structure)
        //{
            
        //}
        //else 
    }



    private void InteractStructure(GameObject gameObject)
    {
        ItemStack itemStack = _player._CurrentItem;

        if (itemStack != null)
        {
            ItemSO currentItem = itemStack._Item;

            if (currentItem != null)
            {
                if (currentItem is StructureSO) return;
            }
        }

        

        Structure structure = gameObject.GetComponent<Structure>();
        Furnace furnace = gameObject.GetComponent<Furnace>();
        if (structure != null && CheckDistance(gameObject))
        {
            _button_structureGroup.SetActive(true);


            _button_activeStructure.SetActive(furnace != null);
        }
        else
        {
            _button_structureGroup.SetActive(false);
            _button_activeStructure.SetActive(false);
        }
    }

    #endregion structure

    private void HideAll()
    {
        _button_structureGroup.SetActive(false);
        _button_activeStructure.SetActive(false);
    }
}
