using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<ToggleType, bool> switchValues = new();
    public Dictionary<string, int> leaderBoard = new();

    public static UIDataManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetValues();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetValues()
    {
        // Load saved settings
        //SaveSystem.Instance.LoadData();

        sliderValues.Add(SliderType.MainVolume, 1);
        sliderValues.Add(SliderType.MusicVolume, 1);
    }
}

public enum SliderType
{
    MainVolume = 0,
    MusicVolume = 1,
}

public enum ToggleType
{
    MainOnOrOff = 0,
    MusicOnOrOff = 1,
}
