using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public enum MoveState { Move, Sprint}

public class InputHandle : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput PlayerInput
    {
        get
        {
            return playerInput;
        }
    }

    #region action map

    private void OnInventory()
    {
        SwitchActionMap(this.InventoryActionMap);
    }

    private void SwitchActionMap(string newActionMap)
    {
        if (this.CurrentActionMap == newActionMap) return;

        Debug.LogWarning($"Switch action map {this.CurrentActionMap} & {newActionMap}");

        this.CurrentActionMap = newActionMap;

        this.PlayerInput.SwitchCurrentActionMap(this.CurrentActionMap);
    }

    private string currentActionMap = "";
    public string CurrentActionMap
    {
        set
        {
            this.currentActionMap = value;
        }
        get
        {
            return this.currentActionMap;
        }
    }

    private string defaultActionMap = "Player";
    public string DefaultActionMap
    {
        get
        {
            return this.defaultActionMap;
        }
    }

    private string inventoryActionMap = "Inventory";
    public string InventoryActionMap
    {
        get
        {
            return this.inventoryActionMap;
        }
    }

    private string notActionMap = "Do not action";
    public string NotActionMap => this.notActionMap;

    #endregion action map

    #region Player action map 
    //player action map
    private InputAction idle;
    private InputAction selectTarget;
    private InputAction setDestination;

    private void RegisterPlayerActionMap()
    {
        idle = PlayerInput.actions["Idle"];
        selectTarget = PlayerInput.actions["Select target"];
        setDestination = PlayerInput.actions["Set destination"];

        RegisterMovementAction();
        RegisterOpenInventoryAction();
        RegisterOpenMenuAction();
        RegisterStructureAction();
        RegisterInteractMapAction();
    }

    public static event Action<bool> OnIdle;
    public static event Action<GameObject> OnSelectTarget;
    public static event Action OnSetDestination;

    #region movement / player action map
    //movement
    private InputAction walk;
    private InputAction run;
    private InputAction sprint;

    private void RegisterMovementAction()
    {
        sprint = playerInput.actions["Sprint"];
    }

    public static event Action OnWalk;
    public static event Action OnRun;
    public static event Action<bool> OnSprint;
    #endregion movement / player action map


    #region structure / player action map
    //structure
    private InputAction structureSetActive;
    private InputAction structureBuilding;
    private InputAction structureDismantle;
    private InputAction structureRepair;

    private void RegisterStructureAction()
    {
        structureSetActive = playerInput.actions["Set active structure"];
        structureBuilding = PlayerInput.actions["Build structure"];
        structureDismantle = PlayerInput.actions["Dismantle structure"];
        structureRepair = PlayerInput.actions["Repair structure"];
    }

    public static event Action OnSetActiveStructure;
    public static event Action OnBuildStructure;
    public static event Action OnRepairStructure;
    public static event Action<bool> OnDismantleStructure;
    #endregion structure / player action map

    #region inventory / player action map
    //inventory
    private InputAction openInventory;

    private void RegisterOpenInventoryAction()
    {
        openInventory = PlayerInput.actions["Open inventory"];
    }

    public static event Action OnOpenInventory;
    #endregion inventory / player action map

    #region map / player action map
    //map
    private InputAction interactMap;

    private void RegisterInteractMapAction()
    {
        interactMap = PlayerInput.actions["Interaction map"];
    }

    public static event Action OnInteractionMap;
    #endregion map / player action map

    #region menu / player action map
    //menu
    private InputAction openMenu;

    private void RegisterOpenMenuAction()
    {
        openMenu = PlayerInput.actions["Open menu"];
    }

    public static event Action OnOpenMenu;
    #endregion menu / player action map

    #endregion Player action map

    #region Inventory action map
    private InputAction closeInventory;

    private void RegisterInventoryActionMap()
    {
        closeInventory = PlayerInput.actions["Close inventory"];
    }

    public static event Action OnCloseInventory;
    #endregion Inventory action map

    #region menu action map
    private InputAction closeMenu;

    private void RegisterMenuActionMap()
    {
        closeMenu = PlayerInput.actions["Close menu"];
    }

    public static event Action OnCloseMenu;
    #endregion menu action map

    #region raycast
    [Header("obj in 3d world")]
    [SerializeField] private LayerMask raycastHitLayer;
    public static event Action<GameObject> OnRaycastHit;

    private void Raycast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, this.raycastHitLayer)
                && this.PlayerInput.currentActionMap.name == this.DefaultActionMap)
        {
            if (hit.collider.isTrigger == true)
            {
                OnRaycastHit?.Invoke(hit.collider.gameObject);
            }
        }
        else OnRaycastHit?.Invoke(null);
    }

    //[Header("item")]
    //[SerializeField] private LayerMask itemLayer;
    public static event Action<ItemSO> OnRaycastItem;
    private EventSystem eventSystem;
    [SerializeField] private GraphicRaycaster raycaster;

    private void RaycastToItem()
    {
        ItemDisplayUI itemDisplay = null;
        if (this.CurrentActionMap == this.InventoryActionMap)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            RaycastResult result = results.Find(x =>
            {
                GameObject go = x.gameObject;
                if (go.GetComponent<ItemDisplayUI>() != null)
                    return true;
                return false;
            });

            if (result.gameObject != null)
            {
                itemDisplay = result.gameObject.GetComponent<ItemDisplayUI>();
                OnRaycastItem?.Invoke(itemDisplay._itemStack._Item);
                return;
            }
        }

        OnRaycastItem?.Invoke(null);

    }


    #endregion

    private void Update()
    {
        Raycast();
        RaycastToItem();
    }

    private void Awake()
    {
        //StartCoroutine(OnRaycast());

        eventSystem = GetComponent<EventSystem>();

        this.playerInput = GetComponent<PlayerInput>();
        SwitchActionMap(this.DefaultActionMap);

        RegisterPlayerActionMap();
        RegisterMenuActionMap();
        RegisterInventoryActionMap();

    }

    private void InPurdah(bool active)
    {
        SwitchActionMap(active ? NotActionMap : DefaultActionMap);
    }

    private void OnEnable()
    {
        PlayerController.OnGameOver += GameOver;

        //HUD - inventory
        HUD.OnInventory += OnInventory;

        idle.performed += IdlePerform;

        selectTarget.performed += SelectTargetPerform;
        selectTarget.performed += MovementPerform;

        setDestination.performed += SetDestinationPerform;
        setDestination.performed += MovementPerform;

        //movement
        sprint.performed += SprintPerform;
        sprint.canceled += SprintRelease;

        //structure buiding
        structureBuilding.performed += StructureBuildingPerform;

        structureSetActive.performed += SetActiveStructurePerform;

        structureDismantle.started += DismantleStructureStart;
        structureDismantle.canceled += DismantleStructureRelease;

        structureRepair.performed += RepairStructurePerform;

        //inventory
        openInventory.performed += OnOpenInventoryPerform;
        closeInventory.performed += OnCloseInventoryPerform;

        //interact world map
        interactMap.performed += InteractMapPerform;

        //menu 
        openMenu.performed += OpenMenu;
        closeMenu.performed += CloseMenu;
    }

    private void OnDisable()
    {
        PlayerController.OnGameOver += GameOver;

        //HUD - inventory
        HUD.OnInventory -= OnInventory;

        idle.performed -= IdlePerform;

        selectTarget.performed -= SelectTargetPerform;
        selectTarget.performed -= MovementPerform;

        setDestination.performed -= SetDestinationPerform;
        setDestination.performed -= MovementPerform;

        //movement
        sprint.performed -= SprintPerform;
        sprint.canceled -= SprintRelease;

        //structure buiding
        structureBuilding.performed -= StructureBuildingPerform;

        structureSetActive.performed -= SetActiveStructurePerform;

        structureDismantle.started -= DismantleStructureStart;
        structureDismantle.canceled -= DismantleStructureRelease;

        structureRepair.performed -= RepairStructurePerform;

        //inventory
        openInventory.performed -= OnOpenInventoryPerform;
        closeInventory.performed -= OnCloseInventoryPerform;

        //interact world map
        interactMap.performed -= InteractMapPerform;

        //menu 
        openMenu.performed -= OpenMenu;
        closeMenu.performed -= CloseMenu;
    }

    private void GameOver()
    {
        SwitchActionMap("Game over");
    }

    private void IdlePerform(InputAction.CallbackContext context)
    {
        OnIdle?.Invoke(true);
    }

    #region movement
    private void SetDestinationPerform(InputAction.CallbackContext context)
    {
        OnSetDestination?.Invoke();
    }

    private void MovementPerform(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            OnWalk?.Invoke();
        }
        else if(context.interaction is MultiTapInteraction)
        {
            OnRun?.Invoke();
        }
    }

    private bool sprintState = false;
    private void SprintPerform(InputAction.CallbackContext context)
    {
        OnSprint?.Invoke(true);
        sprintState = true;
    }

    private void SprintRelease(InputAction.CallbackContext context)
    {
        if(sprintState == true)
        {
            OnSprint?.Invoke(false);
            sprintState = false;
        }
    }

    #endregion movement


    #region select target
    private void SelectTargetPerform(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, this.raycastHitLayer)
                && this.PlayerInput.currentActionMap.name == this.DefaultActionMap)
        {
            if (hit.collider.isTrigger == true)
            {
                OnSelectTarget?.Invoke(hit.collider.gameObject);
            }
        }
        else OnSelectTarget?.Invoke(null);
    }

    #endregion select target

    #region structure building

    private void StructureBuildingPerform(InputAction.CallbackContext context)
    {
        OnBuildStructure?.Invoke();
    }

    #endregion structure building

    #region Inventory
    //OpenInventory
    private void OnOpenInventoryPerform(InputAction.CallbackContext context)
    {
        OnOpenInventory?.Invoke();

        SwitchActionMap(this.InventoryActionMap);
    }

    //close inventory
    private void OnCloseInventoryPerform(InputAction.CallbackContext context)
    {
        OnCloseInventory?.Invoke();

        SwitchActionMap(this.DefaultActionMap);
    }

    #endregion Inventory

    #region button suggest

    private void SetActiveStructurePerform(InputAction.CallbackContext context)
    {
        OnSetActiveStructure?.Invoke();
    }

    private void DismantleStructureStart(InputAction.CallbackContext context)
    {
        OnDismantleStructure?.Invoke(true);
    }

    private void DismantleStructureRelease(InputAction.CallbackContext context)
    {
        OnDismantleStructure?.Invoke(false);
    }

    private void RepairStructurePerform(InputAction.CallbackContext context)
    {
        OnRepairStructure?.Invoke();
    }

    #endregion button suggest

    #region map
    private void InteractMapPerform(InputAction.CallbackContext context)
    {
        OnInteractionMap?.Invoke();
    }

    #endregion map

    #region menu
    private void OpenMenu(InputAction.CallbackContext context)
    {
        OnOpenMenu?.Invoke();
        SwitchActionMap("Menu");
    }

    private void CloseMenu(InputAction.CallbackContext context)
    {
        if(context.interaction is TapInteraction)
        {
            OnCloseMenu?.Invoke();
            SwitchActionMap(this.DefaultActionMap);
        }
    }


    #endregion menu
}
