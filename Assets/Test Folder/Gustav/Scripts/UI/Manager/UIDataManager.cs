using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<ToggleType, bool> toggleValues = new();
    public Dictionary<string, int> leaderBoard = new();
    public Data Currentdata { get; private set; }

    public static UIDataManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        SetValues();
    }

    public void SetValues()
    {
        // Load saved settings
        Currentdata = SaveSystem.Instance.LoadData();

        for (int i = 0; i < Currentdata.sliderTypes.Length; i++)
        {
            if (!sliderValues.ContainsKey(Currentdata.sliderTypes[i]))
            {
                sliderValues.Add(Currentdata.sliderTypes[i], Currentdata.sliderValues[i]);
            }
            else
            {
                sliderValues[Currentdata.sliderTypes[i]] = Currentdata.sliderValues[i];
            }
        }

        for (int i = 0; i < Currentdata.toggleTypes.Length; i++)
        {
            if (!toggleValues.ContainsKey(Currentdata.toggleTypes[i]))
            {
                toggleValues.Add(Currentdata.toggleTypes[i], Currentdata.toggleValues[i]);
            }
            else
            {
                toggleValues[Currentdata.toggleTypes[i]] = Currentdata.toggleValues[i];
            }
        }

        for (int i = 0; i < Currentdata.leadBoardNames.Length; i++)
        {
            if (Currentdata.leadBoardNames[i] != null && !leaderBoard.ContainsKey(Currentdata.leadBoardNames[i]))
            {
                leaderBoard.Add(Currentdata.leadBoardNames[i], Currentdata.leaderBoardScore[i]);
            }
            else if (Currentdata.leadBoardNames[i] != null && leaderBoard.ContainsKey(Currentdata.leadBoardNames[i]))
            {
                leaderBoard[Currentdata.leadBoardNames[i]] = Currentdata.leaderBoardScore[i];
            }
        }
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
