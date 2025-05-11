using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class AudioManager : SingletonBase<AudioManager>
{
    private const string VOLUME_PLAYERPREF = "Volume";
    private const float DEFAULT_VOLUME = 0.25f;
    private const float MAX_VOLUME = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(VOLUME_PLAYERPREF) == false)
        {
            PlayerPrefs.SetFloat(VOLUME_PLAYERPREF, DEFAULT_VOLUME);
            PlayerPrefs.Save();
        }

        AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_PLAYERPREF, DEFAULT_VOLUME);
    }

    public void SetVolume(float volume)
    {
        float newVolume = Mathf.Min(volume, MAX_VOLUME);
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat(VOLUME_PLAYERPREF, newVolume);
        PlayerPrefs.Save();
    }
}