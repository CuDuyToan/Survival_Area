using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemInfo : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InputHandle.OnRaycastItem += AssignInformation;
    }

    private void OnDestroy()
    {
        InputHandle.OnRaycastItem -= AssignInformation;
    }

    void Update()
    {
        MoveWithMousePosition();
        DisplayInfo();

    }

    private void MoveWithMousePosition()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out pos);

        this.rectTransform.anchoredPosition = pos;
    }


    #region index

    [Header("Index")]
    private ItemSO itemSO;
    [SerializeField] private GameObject boardInfo;
    [SerializeField] private Text text;

    private string infomation = "";
    private void DisplayInfo()
    {
        if(itemSO != null)
        {
            infomation =
               $"Name [{itemSO.ItemName}] \n" +
               $"Description   {itemSO.ItemDescription}";


            if (itemSO is FoodSO food)
            {
                infomation =
                    $"Name          {food.ItemName} \n" +
                    $"Type          [Food]\n" +
                    $"Food point    {food.FoodPoint} \n" +
                    $"Water point   {food.WaterPoint} \n" +
                    $"Health point  {food.HealthPoint} \n" +
                    $"Description   {food.ItemDescription} \n";
            }
            else if (itemSO is ToolSO tool)
            {
                infomation =
                    $"Name          {tool.ItemName} \n" +
                    $"Type          [Tool] \n" +
                    $"Damage        {tool.BonusDame} \n" +
                    $"Description   {tool.ItemDescription} \n";
            }
            else if (itemSO is WeaponSO weapon)
            {
                infomation =
                    $"Name          {weapon.ItemName} \n" +
                    $"Type          [Weapon] \n" +
                    $"Damage        {weapon.BonusDame} \n" +
                    $"Description   {weapon.ItemDescription} \n";
            }
            else if (itemSO is StructureSO structure)
            {
                infomation =
                    $"Name          {structure.ItemName} \n" +
                    $"Type          [Structure] \n" +
                    $"Description   {structure.ItemDescription} \n";
            }

            text.text = infomation;
        }
        //if (infomation == text.text) return;


        if (itemSO == null)
        {
            boardInfo.SetActive(false);
            return;
        }
        else if (itemSO != null) boardInfo.SetActive(true);

    }

    private void AssignInformation(ItemSO item)
    {
        if (itemSO != item)
        {
            itemSO = item;
        }
    }
    #endregion index
}
