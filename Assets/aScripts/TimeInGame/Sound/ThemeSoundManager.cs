using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ThemeSoundManager : MonoBehaviour
{
    private TimeManager timeManager;

    [Header("day")]
    [SerializeField] private AudioClip daySound;
    [SerializeField, Min(0)] private float dayVolume;

    [Header("night")]
    [SerializeField] private AudioClip nightSound;
    [SerializeField, Min(0)] private float nightVolume;

    private AudioSource audioSource;

    private void Start()
    {
        timeManager = GetComponent<TimeManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if((int)(timeManager.TimeCount / 3600) == 5 || (int)(timeManager.TimeCount / 3600) == 17)
        {
            audioSource.loop = false;
        }
        else
        {
            if (timeManager.TimeCount / 3600 >= 8 && timeManager.TimeCount / 3600 < 17 && !audioSource.isPlaying)
            {
                Debug.Log("day");
                audioSource.clip = daySound;
                audioSource.loop = true;
                audioSource.volume = dayVolume;
                audioSource.Play();
            } 
            else if ((timeManager.TimeCount / 3600 >= 20 || timeManager.TimeCount / 3600 < 5) && !audioSource.isPlaying)
            {
                Debug.Log("night");
                audioSource.clip = nightSound;
                audioSource.loop = true;
                audioSource.volume = nightVolume;
                audioSource.Play();
            }
        }
    }
}
