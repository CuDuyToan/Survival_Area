using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    #region default index
    private float defaultVolumeValue = 1f;
    private float defaultDistance = 1;
    private float defaultSpread = 1;
    #endregion default index

    private void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.defaultVolumeValue = this.audioSource.volume;
        this.defaultDistance = this.audioSource.maxDistance;
        this.defaultSpread = this.audioSource.spread;
    }

    #region random sound

    private List<AudioClip> audioClips = new List<AudioClip>();

    public void AddSoundToList(AudioClip audioClip)
    {
        this.audioClips.Add(audioClip);
    }

    public void PlayRandomSoundInList()
    {
        if (audioClips.Count <= 0) return;

        AudioClip clip = this.audioClips[RandomSystem.RandomInt(this.audioClips.Count-1, 0)];

        this.audioSource.PlayOneShot(clip);

        this.audioClips.Clear();
    }

    #endregion

    public void PlayOneShot(AudioClip audioClip)
    {
        this.audioSource.PlayOneShot(audioClip);
    }

    public void SetSpread(float value)
    {
        if(value > 360) value = 360;
        else if(value < 0) value = 0;

        this.audioSource.spread = value;
    }

    public void ResetSpread()
    {
        this.audioSource.spread = this.defaultSpread;
    }

    public void SetDistance(float value)
    {
        if(value < 1) value = 1;

        this.audioSource.maxDistance = value;
    }

    public void ResetDistance()
    {
        this.audioSource.maxDistance = this.defaultDistance;
    }

    public void SetVolume(float volumeValue)
    {
        if(volumeValue > 1) volumeValue = 1;
        else if(volumeValue < 0) volumeValue = 0;

        float defaultVolume = this.audioSource.volume;
        this.audioSource.volume = volumeValue;

        this.audioSource.volume = defaultVolume;
    }

    public void ResetVolumeValue()
    {
        this.audioSource.volume = this.defaultVolumeValue;
    }
}
