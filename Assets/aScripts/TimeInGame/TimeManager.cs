using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private string currentTime;
    [SerializeField] private float secondCount;
    [SerializeField] private int dayCount;

    private int DayCount
    {
        set
        {
            if(value >= 365)
            {
                dayCount = 0;
            }
            else
            {
                dayCount = value;
            }
        }
        get
        {
            return dayCount;
        }
    }
    public float TimeCount
    {
        set
        {
            int secondsPerDay = 60 * 60 * 24;
            if (value >= secondsPerDay)
            {
                int newDay = (int)(value / secondsPerDay);
                DayCount += newDay;
                secondCount = (int)(value % secondsPerDay);
            }
            else
            {
                secondCount = (int)value;
            }

            SaveAndLoadSystem.timeInGame = secondCount;

        }
        get
        {
            return secondCount;
        }
    }

    [SerializeField, Min(0)] private float xSpeed = 60;

    public static event Action<float, int> TimeOnWorld;

    private void Awake()
    {
        LoadManager.OnLoadTime += LoadTime;
    }

    private void LoadTime(float timeInGame, int totalPlayTime)
    {
        TimeCount = timeInGame;
        SaveAndLoadSystem.timeInGame = timeInGame;

        SaveAndLoadSystem.totalPlayTime = totalPlayTime;
    }

    private void Start()
    {
        //realTime = SaveAndLoadSystem.totalPlayTime;
    }

    private void Update()
    {
        SaveAndLoadSystem.totalPlayTime += Time.deltaTime;
        TimeCount += Time.deltaTime * xSpeed;

        //SaveAndLoadSystem.timeInGame += Time.deltaTime * xSpeed;

        currentTime = FormatTime(TimeCount);

        TimeOnWorld?.Invoke(secondCount, dayCount);
    }

    private void OnEnable()
    {
        Tent.OnSetTime += SetTime;
    }

    private void OnDisable()
    {
        Tent.OnSetTime -= SetTime;
    }

    private void SetTime(float newTime)
    {
        Debug.Log($"set time {newTime}, old time {TimeCount}");
        TimeCount += newTime;
        Debug.Log($"new time {TimeCount}");
    }

    public static string FormatTime(float totalSeconds)
    {
        int hours = (int)(totalSeconds / 3600);
        int minutes = (int)((totalSeconds % 3600) / 60);
        int seconds = (int)(totalSeconds % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

}
