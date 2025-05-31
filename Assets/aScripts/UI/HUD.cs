using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region purdah
    [SerializeField] private Purdah purdah;
    #endregion


    #region menu
    [SerializeField] private GameObject menu;

    private void OpenMenu()
    {
        menu.SetActive(true);
    }

    private void CloseMenu()
    {
        menu.SetActive(false);
    }
    #endregion menu

    #region game over
    [SerializeField] private GameObject gameOver;

    private void DisplayGameOverScene()
    {
        gameOver.SetActive(true);
    }
    #endregion game over

    public static event Action OnInventory;

    private void OnEnable()
    {
        InputHandle.OnRaycastHit += DisplayInfoGameObject;

        InputHandle.OnOpenInventory += OpenInventory;
        InputHandle.OnCloseInventory += CloseInventory;

        InputHandle.OnCloseInventory += CloseAllUI;

        Structure.OnInteractStructure += StrutureInteract;

        InputHandle.OnInteractionMap += InteractMap;

        InputHandle.OnOpenMenu += OpenMenu;
        InputHandle.OnCloseMenu += CloseMenu;

        PlayerController.OnGameOver += DisplayGameOverScene;
    }

    private void OnDisable()
    {
        InputHandle.OnRaycastHit -= DisplayInfoGameObject;

        InputHandle.OnOpenInventory -= OpenInventory;
        InputHandle.OnCloseInventory -= CloseInventory;

        InputHandle.OnCloseInventory -= CloseAllUI;

        Structure.OnInteractStructure -= StrutureInteract;

        InputHandle.OnInteractionMap -= InteractMap;

        InputHandle.OnOpenMenu -= OpenMenu;
        InputHandle.OnCloseMenu -= CloseMenu;

        PlayerController.OnGameOver -= DisplayGameOverScene;

    }
    private void CloseAllUI()
    {
        backGround.SetActive(false);

        _inventory_ItemDisplay.gameObject.SetActive(false);
        _craftingUI.gameObject.SetActive(false);
        _itemStorageUI.gameObject.SetActive(false);

        mapGroup.SetActive(true);
    }

    #region Object info
    [Header("gameobj Info UI")]
    [SerializeField] private GameObject _gameObjInfoUI;

    private void DisplayInfoGameObject(GameObject gameObj)
    {
        if(gameObj == null)
        {
            _gameObjInfoUI.SetActive(false);
            return;
        }
        else if(gameObj != null)
        {
            _gameObjInfoUI.SetActive(true);
            return;
        }
    }

    #endregion


    #region inventoryPlayer
    [Header("player")]
    [SerializeField] private GameObject _inventory_ItemDisplay;
    [SerializeField] private GameObject backGround;
    [SerializeField] private CraftingPlayer _craftingPlayer;

    private void OpenInventory()
    {
        DisplayInventory(true);
        DisplayCraftingUI(true);
    }

    private void CloseInventory()
    {
        DisplayInventory(false);
    }

    private void DisplayInventory(bool state)
    {
        mapGroup.SetActive(!state);
        backGround.SetActive(state);
        _inventory_ItemDisplay.gameObject.SetActive(state);

        if (state == false) CloseAllUI();
    }

    private void DisplayCraftingUI(bool state)
    {
        _craftingUI.SetCraftingBase = _craftingPlayer;
        _craftingUI.gameObject.SetActive(state);
    }

    #endregion

    #region structure interact
    [Header("structure")]
    [SerializeField] CraftingUI _craftingUI;
    [SerializeField] ItemStorageUI _itemStorageUI;

    private void StrutureInteract(Structure structure)
    {
        ItemStorage storage = structure.GetComponent<ItemStorage>();
        _itemStorageUI.SetItemContainer = storage;
        _itemStorageUI.gameObject.SetActive(storage != null);

        CraftingStation station = structure.GetComponent<CraftingStation>();
        _craftingUI.SetCraftingBase = station;
        _craftingUI.gameObject.SetActive(station != null);

        if(storage != null || station != null)
        {
            OnInventory?.Invoke();
            DisplayInventory(true);
        }

        if(structure is Tent tent)
        {
            if(tent.CheckTime())
            {
                purdah.OnOpenPurdah(purdah.DefaultSpeed);
                tent.Sleep();
            }
        }
    }

    #endregion structure interact

    #region interact map
    [Header("Interact map")]
    [SerializeField] private GameObject mapGroup;
    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject fullMap;

    private void InteractMap()
    {
        bool active = miniMap.activeSelf;

        miniMap.SetActive(!active);
        fullMap.SetActive(active);
    }


    #endregion
}
