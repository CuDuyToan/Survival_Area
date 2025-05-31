using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public float health;

    public float timeCount;

    public ResourceData(float health, float timeCount)
    {
        this.health = health;
        this.timeCount = timeCount;
    }
}
