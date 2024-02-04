using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class DataManager : MonoBehaviour
{
    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<SwitchType, bool> switchValues = new();

    private static DataManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}

public enum SliderType
{
    MainVolume,
    MusicVolume,
}

public enum SwitchType
{
    MainOnOrOff,
    MusicOnOrOff,
}
