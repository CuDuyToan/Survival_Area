using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LoadManager : MonoBehaviour
{
    private void Start()
    {
        LoadData();
    }

    public static event Action<float, int> OnLoadTime;

    private void LoadData()
    {
        WorldData worldData = SaveAndLoadSystem.LoadGame();

        if(worldData != null)
        {

            Debug.Log($"time in game : {worldData.timeInGame} - total play time : {worldData.totalPlayTime}");
            OnLoadTime?.Invoke(worldData.timeInGame, worldData.totalPlayTime);

            LoadPlayerData(worldData.playerData);

            LoadCreatureData(worldData.creatureDatas);

            LoadStructureData(worldData.structureDatas);

            LoadResourceData(worldData.resourceDatas);
        }
    }

    #region player

    [SerializeField] private GameObject player;
    [SerializeField] private Transform _camera;

    private void LoadPlayerData(PlayerData data)
    {
        //PlayerData data = SaveAndLoadSystem.LoadPlayer();

        if (data != null)
        {
            player.SetActive(false);

            Vector3 position = new Vector3(data.position.x, data.position.y+2, data.position.z);
            Vector3 rotation = new Vector3(data.rotation.x, data.rotation.y, data.rotation.z);

            player.transform.position = position;

            player.transform.eulerAngles = rotation;

            player.SetActive(true);

            _camera.position = position;

            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController._Health = data.health;
            playerController._Food = data.food;
            playerController._Stamina = data.stamina;
            playerController.Water = data.water;

            InventoryPlayer inventory = player.GetComponent<InventoryPlayer>();

            foreach (ItemData itemData in data.inventoryData)
            {
                inventory.AddNewItemSlot(itemData.ConvertDataToItem());
            }
        }
    }

    #endregion player

    #region creature

    public static event Action<GameObject, CreatureData> OnLoadCreature;

    private void LoadCreatureData(List<CreatureData> data)
    {
        //List<CreatureData> data = SaveAndLoadSystem.LoadCreature();

        CreatureDB creatureDB = Resources.Load<CreatureDB>("CreatureDB");

        if (data != null)
        {
            foreach (CreatureData creaturedata in data)
            {
                if(creaturedata == null) continue;

                GameObject creatureObj = creatureDB.creatureList.Find(member =>
                {
                    Creature creature = member.GetComponent<Creature>();
                    return creature != null && creature._creatureSO.Name == creaturedata.name_SO;
                });

                if (creatureObj == null) continue;

                OnLoadCreature?.Invoke(creatureObj, creaturedata);
            }
        }
    }

    #endregion creature

    #region structure

    public static event Action<GameObject, StructureData> OnLoadStructure;

    private void LoadStructureData(List<StructureData> data)
    {
        //List<StructureData> data = SaveAndLoadSystem.LoadStructure();

        StructureDB structureDB = Resources.Load<StructureDB>("StructureDB");

        if (data != null)
        {
            foreach (StructureData structureData in data)
            {
                if (structureData == null) continue;

                GameObject structureObj = structureDB.StructureList.Find(member =>
                {
                    Structure structure = member.GetComponent<Structure>();
                    return structure != null && structure._StructureSO.ItemName == structureData.nameStructure;
                });
                if (structureObj == null) continue;

                OnLoadStructure?.Invoke(structureObj, structureData);
            }
        }
    }

    #endregion

    #region resource
    [Header("resource")]
    [SerializeField] private List<Resource> bushList = new List<Resource>();
    [SerializeField] private List<Resource> treeList = new List<Resource>();
    [SerializeField] private List<Resource> stoneList = new List<Resource>();
    [SerializeField] private List<Resource> oreList = new List<Resource>();

    private List<Resource> ResourceList
    {
        get
        {
            List<Resource> resourceList = new List<Resource>();

            resourceList.Clear();

            resourceList.AddRange(bushList);
            resourceList.AddRange(treeList);
            resourceList.AddRange(stoneList);
            resourceList.AddRange(oreList);

            return resourceList;
        }
    }

    private void LoadResourceData(List<ResourceData> data)
    {
        //List<ResourceData> data = SaveAndLoadSystem.LoadResource();

        if (data != null)
        {
            if (ResourceList.Count < data.Count)
            {
                Debug.Log("resources list < data");
                return;
            }

            for (int i = 0; i < data.Count; i++)
            {
                ResourceList[i]._Health = data[i].health;
                ResourceList[i]._TimeCount = data[i].timeCount;
            }
        }
    }

    #endregion resource
}
