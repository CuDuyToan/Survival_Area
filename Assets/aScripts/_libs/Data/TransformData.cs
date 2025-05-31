using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z; 
    
    public PositionData(Vector3 position)
    {
        this.x = position.x;

        this.y = position.y;

        this.z = position.z;
    }
}

[System.Serializable]
public class RotationData
{
    public float x;
    public float y;
    public float z;

    public RotationData(Vector3 rotation)
    {
        this.x = rotation.x;
        this.y = rotation.y;
        this.z = rotation.z;
    }
}
