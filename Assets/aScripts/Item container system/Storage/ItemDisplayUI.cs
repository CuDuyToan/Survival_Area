using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplayUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region display member
    [Header("Display")]
    [SerializeField] private Image _background;
    //sprite
    [SerializeField] private Image _image;
    //quantity
    [SerializeField] private Text _countText;
    // durability
    [SerializeField] private GameObject _durabilityGr;
    [SerializeField] private Image _durabilityBar;

    #endregion

    #region data member
    [Header("Data")]
    /*[HideInInspector]*/ public ItemStack _itemStack;

    [HideInInspector] public Transform _parentAfterDrag;

    #endregion

    private void Start()
    {
        InitaliseItem(_itemStack);
    }

    private void Update()
    {
        RefreshParameters();

        CheckToDestroy();

    }

    public void InitaliseItem(ItemStack newItem)
    {
        _itemStack = newItem;
        _image.sprite = newItem._Item.ItemSprite;
        _parentAfterDrag = this.transform.parent;
        RefreshParameters();
    }

    public void OnClickThisItem()
    {
        ItemContainLink containLink = _parentAfterDrag.GetComponent<ItemContainLink>();

        if (containLink == null) return;

        if(containLink.InventoryContainer is InventoryPlayer inventoryPlayer)
        {
            Consume(inventoryPlayer);

            Equip(inventoryPlayer);

        }
    }

    #region Using

    private void Consume(InventoryPlayer inventoryPlayer)
    {
        if (_itemStack._Item is FoodSO food)
        {
            inventoryPlayer.ConsumeThisItem(_itemStack);

            PlayerController player = inventoryPlayer.GetComponent<PlayerController>();
            player.Eat(food);
        }
    }

    private void Equip(InventoryPlayer inventoryPlayer)
    {
        ItemSO item = _itemStack._Item;

        if (item is ToolSO || item is StructureSO || item is WeaponSO)
        {
            inventoryPlayer.SelectItem(_itemStack);
        }
    }

    #endregion

    #region Index

    public void RefreshParameters()
    {
        GetIndex();

        _durabilityBar.fillAmount = DurabilityCalculation();
        bool durBarActive = _itemStack._maxDurability > 1;
        _durabilityGr.SetActive(durBarActive);

        _countText.text = _itemStack._Quantity.ToString();
        bool textActive = _itemStack._Quantity > 1;
        _countText.gameObject.SetActive(textActive);
    }

    public void CheckToDestroy()
    {
        if (_itemStack._Item.Stackable && _itemStack._Quantity == 0)
        {
            Destroy(this.gameObject);
        }
        else if (!_itemStack._Item.Stackable && _itemStack._Durability == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetIndex()
    {
        if (!_itemStack._Item) return;
        ToolSO tool = _itemStack._Item as ToolSO;
        if (tool != null)
        {
            _itemStack._maxDurability = tool.MaxDurrability;
            //_itemStack._Durability = tool.CurrentDurability;
        }
    }

    private float DurabilityCalculation()
    {
        if (_itemStack._maxDurability == 0) return 0;
        return (_itemStack._Durability / _itemStack._maxDurability);
    }

    #endregion

    #region Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        DisplayeBackGround(false);

        _background.raycastTarget = false;
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DisplayeBackGround(true);

        _background.raycastTarget = true;
        transform.SetParent(_parentAfterDrag);
    }

    private void DisplayeBackGround(bool state)
    {
        Color colorImg = _background.color;

        float rate = state ? 0.5f : 0f;

        colorImg.a = rate;

        _background.color = colorImg;
    }

    #endregion

}
