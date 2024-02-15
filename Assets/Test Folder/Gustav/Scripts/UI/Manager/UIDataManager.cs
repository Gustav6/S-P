using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIDataManager : MonoBehaviour
{
    public static UIDataManager instance = null;

    public Dictionary<SliderType, float> sliderValues = new();
    public Dictionary<ToggleType, bool> toggleValues = new();
    public Dictionary<string, int> scoreLeaderBoard = new();
    public Dictionary<string, int> waveLeaderBoard = new();
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

    public void SetValues()
    {
        // Load saved settings
        CurrentData = SaveSystem.Instance.LoadData();

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

        for (int i = 0; i < CurrentData.scoreLeadBoardNames.Length; i++)
        {
            if (CurrentData.scoreLeadBoardNames[i] != null)
            {
                scoreLeaderBoard.TryAdd(CurrentData.scoreLeadBoardNames[i], CurrentData.scoreLeaderBoardValues[i]);
            }
            else
            {
                scoreLeaderBoard.Add("" + i, 0);
            }
            //scoreLeaderBoard[CurrentData.scoreLeadBoardNames[i]] = CurrentData.scoreLeaderBoardValues[i];
            //if (CurrentData.scoreLeadBoardNames[i] != null && !scoreLeaderBoard.ContainsKey(CurrentData.scoreLeadBoardNames[i]))
            //{
            //    scoreLeaderBoard.Add(CurrentData.scoreLeadBoardNames[i], CurrentData.scoreLeaderBoardValues[i]);
            //}
            //else if (CurrentData.scoreLeadBoardNames[i] != null && scoreLeaderBoard.ContainsKey(CurrentData.scoreLeadBoardNames[i]))
            //{
            //    scoreLeaderBoard[CurrentData.scoreLeadBoardNames[i]] = CurrentData.scoreLeaderBoardValues[i];
            //}
        }

        for (int i = 0; i < CurrentData.waveLeadBoardNames.Length; i++)
        {
            if (CurrentData.waveLeadBoardNames[i] != "")
            {
                waveLeaderBoard.TryAdd(CurrentData.waveLeadBoardNames[i], CurrentData.waveLeaderBoardValues[i]);
            }
            else
            {
                waveLeaderBoard.Add("" + i, 0);
            }

            //waveLeaderBoard[CurrentData.waveLeadBoardNames[i]] = CurrentData.waveLeaderBoardValues[i];
            //if (CurrentData.waveLeadBoardNames[i] != null && !waveLeaderBoard.ContainsKey(CurrentData.waveLeadBoardNames[i]))
            //{
            //    waveLeaderBoard.Add(CurrentData.waveLeadBoardNames[i], CurrentData.waveLeaderBoardValues[i]);
            //}
            //else if (CurrentData.waveLeadBoardNames[i] != null && waveLeaderBoard.ContainsKey(CurrentData.waveLeadBoardNames[i]))
            //{
            //    waveLeaderBoard[CurrentData.waveLeadBoardNames[i]] = CurrentData.waveLeaderBoardValues[i];
            //}
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
