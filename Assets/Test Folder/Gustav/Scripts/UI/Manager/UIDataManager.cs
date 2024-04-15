using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms.Impl;

public class UIDataManager : MonoBehaviour
{
    public static UIDataManager instance = null;

    [SerializeField] private AudioMixer audioMixer;

    private const string MIXER_SFX = "SFXVolume";

    private const string MIXER_Music = "MusicVolume";

    private const string MIXER_Master = "MasterVolume";

    public bool hasRunStartScreen = false;

    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<ToggleType, bool> toggleValues = new();

    public int[] score;
    public string[] scoreNames;

    public int[] wave;
    public string[] waveNames;

    public Data CurrentData { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        SetValues();
    }

    public void Start()
    {
        SetMasterVolume(sliderValues[SliderType.MainVolume]);
        SetMusicVolume(sliderValues[SliderType.MusicVolume]);
        SetSFXVolume(sliderValues[SliderType.SfxVolume]);
    }

    public void SetValues()
    {
        // Load saved settings
        CurrentData = SaveSystem.Instance.LoadData();

        wave = new int[CurrentData.waveLeaderBoardValues.Length];
        waveNames = new string[CurrentData.waveLeadBoardNames.Length];
        score = new int[CurrentData.scoreLeaderBoardValues.Length];
        scoreNames = new string[CurrentData.scoreLeadBoardNames.Length];

        for (int i = 0; i < CurrentData.sliderTypes.Length; i++)
        {
            if (!sliderValues.ContainsKey(CurrentData.sliderTypes[i]))
            {
                sliderValues.Add(CurrentData.sliderTypes[i], CurrentData.sliderValues[i]);
            }
            else
            {
                sliderValues[CurrentData.sliderTypes[i]] = CurrentData.sliderValues[i];
            }
        }

        for (int i = 0; i < CurrentData.toggleTypes.Length; i++)
        {
            if (!toggleValues.ContainsKey(CurrentData.toggleTypes[i]))
            {
                toggleValues.Add(CurrentData.toggleTypes[i], CurrentData.toggleValues[i]);
            }
            else
            {
                toggleValues[CurrentData.toggleTypes[i]] = CurrentData.toggleValues[i];
            }
        }

        for (int i = 0; i < scoreNames.Length; i++)
        {
            if (CurrentData.scoreLeadBoardNames[i] != null)
            {
                scoreNames[i] = CurrentData.scoreLeadBoardNames[i];
                score[i] = CurrentData.scoreLeaderBoardValues[i];
            }
            else
            {
                scoreNames[i] = "";
                score[i] = 0;
            }
        }

        for (int i = 0; i < waveNames.Length; i++)
        {
            if (CurrentData.waveLeadBoardNames[i] != null)
            {
                waveNames[i] = CurrentData.waveLeadBoardNames[i];
                wave[i] = CurrentData.waveLeaderBoardValues[i];
            }
            else
            {
                waveNames[i] = "";
                wave[i] = 0;
            }
        }
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MIXER_Master, Mathf.Log10(value) * 20);
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_Music, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}

public enum SliderType
{
    MainVolume = 0,
    MusicVolume = 1,
    SfxVolume = 2,
}

public enum ToggleType
{
    MainOnOrOff = 0,
    MusicOnOrOff = 1,
    SfxOnOrOff = 2,
}
