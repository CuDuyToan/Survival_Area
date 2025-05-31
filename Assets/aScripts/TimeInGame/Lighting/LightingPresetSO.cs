using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = ("Lighting Preset"), menuName = ("Scriptable/Time/Lighting Preset"))]
public class LightingPresetSO : ScriptableObject
{
    public Gradient AmbienColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
