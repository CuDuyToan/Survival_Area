using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DisplayObjInfo : MonoBehaviour
{
    #region display
    [SerializeField, Header("Name")] private Text _name;

    [SerializeField, Header("Health bar")] private GameObject _barObjUI;

    [SerializeField] private Image _bar;

    [SerializeField] private Text _health;

    [SerializeField, Header("Time count")] private GameObject _timeObjUI;
    
    [SerializeField] private Text _timeCount;

    #endregion

    private void OnEnable()
    {
        InputHandle.OnRaycastHit += HandleGameObject;
    }

    private void OnDisable()
    {
        InputHandle.OnRaycastHit -= HandleGameObject;
    }

    private void HideAll()
    {
        _name.enabled = false;
        _barObjUI.SetActive(false);

        _timeObjUI.SetActive(false);
    }

    private void DisplayAll()
    {
        _name.enabled = true;

        _barObjUI.SetActive(true);
    }

    private void HandleGameObject(GameObject gameObject)
    {
        HideAll();
        if (gameObject == null)
        {
            DisplayInfo("name", 0, 1);
            return;
        }

        DisplayAll();

        Structure structure = gameObject.GetComponent<Structure>();
        if (structure)
        {
            StructureInfo(structure);
            return;
        }

        Creature creature = gameObject.GetComponent<Creature>();
        if (creature)
        {
            CreatureInfo(creature);
            return;
        }

        Resource resource = gameObject.GetComponent<Resource>();
        if (resource) 
        { 
            ResourceInfo(resource);
            return;
        }
    }

    private void DisplayInfo(string name, float currentHp, float maxHp)
    {
        string healthRate_text = currentHp.ToString() + "/" + maxHp.ToString();

        float healtRate_data = currentHp / maxHp;

        _name.text = name;
        _health.text = healthRate_text;

        _bar.fillAmount = healtRate_data;
    }

    #region time

    private void DisplayTimeCount(float timeCount)
    {
        _barObjUI.SetActive(false);
        _timeObjUI.SetActive(true);

        string timeDisplay = timeCount.ToString();

        _timeCount.text = FormatTime(timeCount);
    }

    public static string FormatTime(float totalSeconds)
    {
        int hours = (int)(totalSeconds / 3600);
        int minutes = (int)((totalSeconds % 3600) / 60);
        int seconds = (int)(totalSeconds % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    #endregion

    private void StructureInfo(Structure structure)
    {
        float currentHp = structure._Health;
        float maxHp = structure._StructureSO.MaxHealth;

        DisplayInfo(structure._StructureSO.ItemName, currentHp, maxHp);
    }

    private void CreatureInfo(Creature creature)
    {
        float currentHp = creature._Health;
        float maxHp = creature.MaxHealth;

        DisplayInfo(creature._creatureSO.Name, currentHp, maxHp);
    }

    private void ResourceInfo(Resource resource)
    {
        float currentHp = resource._Health;
        float maxHp = resource._resourceSO.MaxHealth;

        string itemInBox = "";

        if(resource is ItemBox itemBox)
        {
            itemInBox = $"({itemBox.Item.ItemName} *{itemBox.Quantity})";
        }

        DisplayInfo($"{resource._resourceSO.ResourceName} {itemInBox}", currentHp, maxHp);

        _barObjUI.SetActive(false);

        if (resource._IsCanRecovery == false) DisplayTimeCount(resource._TimeCount);
    }
}
