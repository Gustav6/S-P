using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<ToggleType, bool> switchValues = new();
    public Dictionary<string, int> leaderBoard = new();

    private static UIDataManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetValues();
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void SetValues()
    {
        // Load saved settings
        sliderValues.Add(SliderType.MainVolume, 1);
        sliderValues.Add(SliderType.MusicVolume, 1);
    }
}

public enum SliderType
{
    MainVolume,
    MusicVolume,
}

public enum ToggleType
{
    MainOnOrOff,
    MusicOnOrOff,
}
