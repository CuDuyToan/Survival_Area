using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomSystem
{
    public static float RandomFloat(float max, float min)
    {
        return Random.Range(max, min);
    }

    public static int RandomInt(int max, int min)
    {
        return Random.Range(max, min);
    }
}
