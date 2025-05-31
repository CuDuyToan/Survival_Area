using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadSystem
{
    #region world

    public static string folderPath = Path.Combine(Application.persistentDataPath, "WorldData");

    public static float totalPlayTime = 0;
    public static float timeInGame = 0;

    public static string SavePath
    {
        get
        {
            return Path.Combine(folderPath, $"world_{worldID}.binary");
        }
    }
    public static string worldID = "0";

    public static void SaveGame(WorldData data)
    {
        data.totalPlayTime = (int) totalPlayTime;
        data.timeInGame = timeInGame;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        //string fullPath = Path.Combine(folderPath, $"world_{worldID}.binary");

        //string path = $"{savePath}/world_{instantID}.binary";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldData LoadGame()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        //string fullPath = Path.Combine(folderPath, $"world_{worldID}.binary");

        //string path = $"{savePath}/world_{instantID}.binary";

        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);
            WorldData data = formatter.Deserialize(stream) as WorldData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogWarning("Save file not found at: " + fullPath);

            FileStream stream = new FileStream(SavePath, FileMode.Create);
            stream.Close();

            return null;
        }
    }

    #endregion


    #region player

    private static string playerDataPath = Application.persistentDataPath + "/player.binary";

    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(playerDataPath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        //Debug.Log("Game saved at: " + playerDataPath);
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(playerDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(playerDataPath, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + creatureDataPath);
            return null;
        }
    }

    #endregion player


    #region creature
    private static string creatureDataPath = Application.persistentDataPath + "/creature.binary";

    public static void SaveCreature(List<CreatureData> data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(creatureDataPath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        //Debug.Log("Game saved at: " + creatureDataPath);
    }

    public static List<CreatureData> LoadCreature()
    {
        if (File.Exists(creatureDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(creatureDataPath, FileMode.Open);
            List<CreatureData> data = formatter.Deserialize(stream) as List<CreatureData>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + creatureDataPath);
            return null;
        }
    }

    #endregion creature

    #region structure

    public static string structureDataPath = Application.persistentDataPath + "/structure.binary";

    public static void SaveStructure(List<StructureData> data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(structureDataPath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();

        //Debug.Log("Game saved at: " + structureDataPath);
    }

    public static List<StructureData> LoadStructure()
    {
        if (File.Exists(structureDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(structureDataPath, FileMode.Open);
            List<StructureData> data = formatter.Deserialize(stream) as List<StructureData>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + structureDataPath);
            return null;
        }
    }

    #endregion structure

    #region resource

    public static string resourceSavePath = Application.persistentDataPath + "/resource.binary";

    public static void SaveResource(List<ResourceData> data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(resourceSavePath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();

        //Debug.Log("Game saved at: " + resourceSavePath);
    }

    public static List<ResourceData> LoadResource()
    {
        if (File.Exists(resourceSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(resourceSavePath, FileMode.Open);
            List<ResourceData> data = formatter.Deserialize(stream) as List<ResourceData>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + resourceSavePath);
            return null;
        }
    }

    #endregion resource
}
