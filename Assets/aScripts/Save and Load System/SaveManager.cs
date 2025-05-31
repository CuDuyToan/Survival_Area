using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    [Header("time")]
    [SerializeField] private ETimeScale timeScale = ETimeScale.Minute;
    private CaculatorTime scale;

    [SerializeField] private float time = 20;
    private float timeCount = 0;

    private void Awake()
    {
        SpawnObjectManager.OnSpawnNewCreature += AddCreatureToList;
        SpawnObjectManager.OnSpawnNewStructure += AddStructureToList;

        PlayerController.OnGameOver += GameOver;

        scale = new CaculatorTime(timeScale);
    }

    private bool isGameOver = false;

    private void GameOver()
    {
        isGameOver = true;
    }

    private void Start()
    {
        Invoke(nameof(SaveGame), 1);
        //SaveGame();
    }

    private void OnDisable()
    {
        SpawnObjectManager.OnSpawnNewCreature -= AddCreatureToList;
        SpawnObjectManager.OnSpawnNewStructure -= AddStructureToList;

        PlayerController.OnGameOver -= GameOver;
    }

    private void Update()
    {
        if(timeCount > time * scale.timeScale)
        {
            SaveGame();

            timeCount = 0;
        }

        timeCount += Time.deltaTime;
    }

    public void SaveGame()
    {
        WorldData worldData = new WorldData();

        worldData.end = isGameOver;

        worldData.playerData = SavePlayerData();

        worldData.creatureDatas = CreatureSaveData();

        worldData.structureDatas = StructureSaveData();

        worldData.resourceDatas = ResourceSaveData();

        SaveAndLoadSystem.SaveGame(worldData);
    }

    #region player
    [Header("Player")]
    [SerializeField] private GameObject player;

    private PlayerData SavePlayerData()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        InventoryPlayer inventory = player.GetComponent<InventoryPlayer>();

        PlayerData data = new PlayerData(playerController, player.transform, inventory);

        //SaveAndLoadSystem.SavePlayer(data);

        return data;
    }



    #endregion player




    #region creature
    [Header("creature")]
    [SerializeField] private List<GameObject> creatureList = new List<GameObject>();

    private void AddCreatureToList(GameObject creature)
    {
        creatureList.Add(creature);
    }

    private List<CreatureData> CreatureSaveData()
    {
        List<CreatureData> data = new List<CreatureData>();

        foreach (GameObject gameObject in creatureList)
        {
            if (gameObject == null) continue;

            Creature creature = gameObject.GetComponent<Creature>();

            Creature_SpawnDeadbody deadbody = creature.GetComponent<Creature_SpawnDeadbody>();

            CreatureData creatureData = new CreatureData(creature, gameObject.transform, deadbody);

            data.Add(creatureData);

        }


        //SaveAndLoadSystem.SaveCreature(data);

        return data;
    }

    #endregion creature

    #region structure

    [Header("structure")]

    [SerializeField] private List<GameObject> structureList = new List<GameObject>();

    private void AddStructureToList(GameObject structure)
    {
        structureList.Add(structure);
    }

    private List<StructureData> StructureSaveData()
    {
        List<StructureData> data = new List<StructureData>();

        foreach (GameObject gameObject in structureList)
        {
            if (gameObject == null) continue;

            Structure structure = gameObject.GetComponent<Structure>();
            
            StructureData structureData = new StructureData(structure, gameObject.transform);


            ItemStorage itemStorage = structure.GetComponent<ItemStorage>();

            if(itemStorage)
            {
                structureData.SaveItemStorage(itemStorage.ItemList);
            }

            data.Add(structureData);

        }


        //SaveAndLoadSystem.SaveStructure(data);

        return data;
    }

    #endregion structure

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


    private List<ResourceData> ResourceSaveData()
    {
        List<ResourceData> data = new List<ResourceData>();

        foreach (Resource resource in ResourceList)
        {
            if (gameObject == null) continue;

            ResourceData resourceData = new ResourceData(resource._Health , resource._TimeCount);

            data.Add(resourceData);

        }


        //SaveAndLoadSystem.SaveResource(data);

        return data;
    }

    #endregion resource
}
