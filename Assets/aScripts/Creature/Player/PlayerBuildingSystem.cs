using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBuildingSystem : MonoBehaviour
{

    private PlayerController _player;

    [SerializeField] private LayerMask _buildingLayer;

    [SerializeField] private Transform _objPreview_Group;
    private GameObject _objPreview;
    private GameObject ObjBuilding
    {
        get
        {
            if (_player._CurrentItem != null)
            {
                if(_player._CurrentItem._Item is StructureSO structure)
                {
                    return structure.StructurePrefab;
                }
            }

            return null;
        }
    }

    private void SetPreviewObj(ItemStack item)
    {

        foreach (Transform objPreview in _objPreview_Group)
        {
            objPreview.gameObject.SetActive(false);
        }

        _objPreview = null;

        if (item == null) return;

        if (item._Item is StructureSO structure)
        {
            foreach (Transform objPreview in _objPreview_Group)
            {
                if(objPreview.name == structure.ObjPreview.name)
                {
                    objPreview.gameObject.SetActive(true);

                    _objPreview = objPreview.gameObject;
                }
            }


        }
    }

    //building
    //private Vector3 _buildingPosition = Vector3.zero;
    //private Vector3 _buildingRotation = Vector3.zero;


    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        InventoryPlayer.OnSelectItem += SetPreviewObj;

        InputHandle.OnBuildStructure += Build;
    }

    private void OnDisable()
    {
        InventoryPlayer.OnSelectItem -= SetPreviewObj;

        InputHandle.OnBuildStructure -= Build;
    }

    #region Building system

    public static event Action<GameObject, Vector3, Vector3> OnBuildingStructure;

    private void Build()
    {
        if (_objPreview && !EventSystem.current.IsPointerOverGameObject())
        {
            ObjBuildingPreview objBuildingPreview = _objPreview.GetComponent<ObjBuildingPreview>();
            if (!objBuildingPreview) return;

            if (objBuildingPreview._IsCanBuild)
            {
                OnBuildingStructure?.Invoke(ObjBuilding, _objPreview.transform.position, _objPreview.transform.eulerAngles);

                _player._inventory.ConsumeThisItem(_player._CurrentItem);

                SetPreviewObj(_player._CurrentItem);
            }
            else
            {
                Debug.Log("can't build this obj");
            }
        }
        else if(EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("to UI");
        }
    }



    #endregion

    private void Update()
    {
        RaycastPreview();
    }

    #region preview

    private void RaycastPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _buildingLayer))
        {
            MoveObjPreview(hit.point);
        }
    }

    private void MoveObjPreview(Vector3 position)
    {
        _objPreview_Group.position = position;

    }

    #endregion preview
}
