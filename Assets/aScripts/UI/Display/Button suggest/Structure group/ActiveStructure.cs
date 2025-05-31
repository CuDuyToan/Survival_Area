using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStructure : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private GameObject _structure;


    private void OnEnable()
    {
        InputHandle.OnSetActiveStructure += PressButton;
        InputHandle.OnRaycastHit += SetStructureObj;
    }

    private void OnDisable()
    {
        InputHandle.OnSetActiveStructure -= PressButton;
        InputHandle.OnRaycastHit -= SetStructureObj;
    }

    private void SetStructureObj(GameObject gameObject)
    {
        _structure = gameObject;
    }

    private void PressButton()
    {
        SetActive(_structure);
    }

    public void SetActive(GameObject target)
    {
        Furnace furnace = target.GetComponent<Furnace>();

        if (furnace != null)
        {
            if(!furnace.IsActive) furnace.OnStartBaking();
            else furnace.OnEndBaking();
        }
    }
}
