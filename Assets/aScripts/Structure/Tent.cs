using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : Structure
{
    [SerializeField, Min(60 * 60 * 3)] private float sleepTime = 60 * 60 * 9;
    public float SleepTime => sleepTime;

    private float second = 0;

    protected override void Start()
    {
        base.Start();

        TimeManager.TimeOnWorld += GetTime;
    }

    private void GetTime(float second, int day)
    {
        this.second = second;
    }

    public static event Action<float> OnSetTime;
    public bool CheckTime()
    {
        if (second / 60 / 60 > 20 || second /60/60 < 3)
        {
            return true;
        }

        return false;
    }

    public void Sleep()
    {
        float sleepValue = sleepTime;
        if(second + sleepTime > 60*60*8)
        {
            sleepValue = 60 * 60 * 8;
        }

        StartCoroutine(SetTime(sleepValue));
    }

    private IEnumerator SetTime(float sleepValue)
    {
        yield return new WaitForSeconds(0.5f);
        OnSetTime?.Invoke(sleepValue);
    }

}
