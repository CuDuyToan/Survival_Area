using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Transform lightTransform;
    //[SerializeField] private Light light;

    private void OnEnable()
    {
        TimeManager.TimeOnWorld += LightOfTime;
    }

    private void OnDisable()
    {
        TimeManager.TimeOnWorld -= LightOfTime;
    }

    private void LightOfTime(float second, int day)
    {
        float hourOnDay = 24 * 60 * 60;

        float x = 360 * (second / hourOnDay);
        float y = 360 * ((day + second/hourOnDay) / 365);

        lightTransform.eulerAngles = new Vector3(x - 90, y, 0);
    }
}
