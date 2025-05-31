using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public string worldName = "World";

    #region time

    public bool end = false;

    public int totalPlayTime = 0;
    public float timeInGame = 0;

    #endregion time

    public PlayerData playerData = null;

    public List<CreatureData> creatureDatas = new List<CreatureData>();

    public List<StructureData> structureDatas = new List<StructureData>();

    public List<ResourceData> resourceDatas = new List<ResourceData>();
}
