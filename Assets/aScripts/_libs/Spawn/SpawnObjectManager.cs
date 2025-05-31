using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnObjectManager : MonoBehaviour
{
    private void OnEnable()
    {
        LoadManager.OnLoadCreature += LoadCreatureFromData;
        LoadManager.OnLoadStructure += LoadStructureFromData;

        PlayerBuildingSystem.OnBuildingStructure += SpawnStructure;
        BiomSpawnCreature.OnSpawnNewCreature += SpawnCreature;

    }

    private void OnDisable()
    {
        LoadManager.OnLoadCreature -= LoadCreatureFromData;
        LoadManager.OnLoadStructure -= LoadStructureFromData;

        PlayerBuildingSystem.OnBuildingStructure -= SpawnStructure;
        BiomSpawnCreature.OnSpawnNewCreature -= SpawnCreature;
    }



    #region creature
    [Header("Creature")]
    [SerializeField] private string creatureTag = "Creature";
    [SerializeField] private Transform creatureGroup;
    public static event Action<GameObject> OnSpawnNewCreature;

    private void SpawnCreature(GameObject Prefab, Vector3 position, Vector3 rotation)
    {
        GameObject newObj = Instantiate(Prefab, creatureGroup);
        newObj.SetActive(false);

        Creature creature = Prefab.GetComponent<Creature>();

        if (newObj != null && creature != null && creatureGroup.childCount < 1000)
        {
            //position.y += 0.001f;

            newObj.transform.position = position;
            newObj.transform.eulerAngles = rotation;

            newObj.SetActive(true);

            //creature.SetIndex();

            newObj.name = $"{creature._creatureSO.name} [{creatureTag}]";

            OnSpawnNewCreature?.Invoke(newObj);
        }
    }

    private void LoadCreatureFromData(GameObject prefabObj, CreatureData creaturedata)
    {
        GameObject newObj = Instantiate(prefabObj, creatureGroup);
        newObj.SetActive(false);

        Vector3 position = new Vector3(creaturedata.position.x, creaturedata.position.y, creaturedata.position.z);

        newObj.transform.position = position;

        Vector3 rotation = new Vector3(creaturedata.rotation.x, creaturedata.rotation.y, creaturedata.rotation.z);
        newObj.transform.eulerAngles = rotation;

        Creature creature = newObj.GetComponent<Creature>();


        if (creature)
        {
            creature._Health = creaturedata.health;
            creature._Food = creaturedata.food;

            newObj.name = $"{creature._creatureSO.name} [{creatureTag}]";


            Creature_SpawnDeadbody deadbody = creature.GetComponent<Creature_SpawnDeadbody>();

            deadbody.DeadBody._Health = creaturedata.deadBody_Hp;
            deadbody.DeadBody._TimeCount = creaturedata.deadBody_Timecount;

            newObj.SetActive(true);

            OnSpawnNewCreature?.Invoke(newObj);

            return;
        }

        Destroy(newObj);
    }
    #endregion

    #region structure
    [Header("Structure")]
    [SerializeField] private string structureTag = "Structure";
    [SerializeField] private Transform structureGroup;
    public static event Action<GameObject> OnSpawnNewStructure;

    private void SpawnStructure(GameObject Prefab, Vector3 position, Vector3 rotation)
    {
        GameObject newObj = Instantiate(Prefab, structureGroup);
        Structure structure = Prefab.GetComponent<Structure>();

        if (newObj != null && structure != null)
        {
            newObj.transform.position = position;
            newObj.transform.eulerAngles = rotation;

            //creature.SetIndex();

            newObj.name = $"{structure._StructureSO.ItemName} [{creatureTag}]";

            OnSpawnNewStructure?.Invoke(newObj);
        }
    }

    private void LoadStructureFromData(GameObject prefabObj, StructureData structureData)
    {
        GameObject newObj = Instantiate(prefabObj, structureGroup);

        Vector3 position = new Vector3(structureData.position.x, structureData.position.y, structureData.position.z);
        newObj.transform.position = position;

        Vector3 rotation = new Vector3(structureData.rotation.x, structureData.rotation.y, structureData.rotation.z);
        newObj.transform.eulerAngles = rotation;

        //Debug.Log($" pos : {position.x} {position.y} {position.z} rotation : {rotation.x} {rotation.y} {rotation.z}");

        Structure structure = newObj.GetComponent<Structure>();


        if (structure)
        {
            structure._Health = structureData.health;

            ItemStorage itemStorage = structure.GetComponent<ItemStorage>();

            if (itemStorage != null)
            {
                itemStorage.LoadItemData(LoadItemList(structureData.itemStorage));
            }

            structure.name = $"{structure._StructureSO.ItemName} [{structureTag}]";

            OnSpawnNewStructure?.Invoke(newObj);

            return;
        }

        Destroy(newObj);
    }
    #endregion

    #region item

    public ItemStack LoadItemData(string itemName, int quantity, float durability)
    {
        ItemDB itemDB = Resources.Load<ItemDB>("ItemDB");

        ItemSO itemSO = itemDB.ItemList.Find(member => member.ItemName == itemName);

        ItemStack itemStack = new ItemStack(itemSO, quantity);

        if(durability > 0)
        {
            itemStack._Durability = durability;
        }

        return itemStack;
    }

    public List<ItemStack> LoadItemList(List<ItemData> itemDatas)
    {
        List<ItemStack> list = new List<ItemStack>();

        if (itemDatas == null) return list;

        foreach (ItemData itemData in itemDatas)
        {
            ItemStack itemStack = itemData.ConvertDataToItem();

            list.Add(itemStack);
        }

        return list;
    }

    #endregion
}
