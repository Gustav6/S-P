using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public Data()
    {
        #region Set array length
        sliderTypes = new SliderType[Enum.GetValues(typeof(SliderType)).Length];
        sliderValues = new float[Enum.GetValues(typeof(SliderType)).Length];

        toggleTypes = new ToggleType[Enum.GetValues(typeof(ToggleType)).Length];
        toggleValues = new bool[Enum.GetValues(typeof(ToggleType)).Length];

        scoreLeadBoardNames = new string[5];
        scoreLeaderBoardValues = new int[5];

        waveLeadBoardNames = new string[5];
        waveLeaderBoardValues = new int[5];
        #endregion

        #region Set default values
        for (int i = 0; i < Enum.GetValues(typeof(SliderType)).Length; i++)
        {
            sliderTypes[i] = (SliderType)i;
            sliderValues[i] = 1;
        }

        for (int i = 0; i < Enum.GetValues(typeof(ToggleType)).Length; i++)
        {
            toggleTypes[i] = (ToggleType)i;
            toggleValues[i] = true;
        }

        SkipTutorial = false;
        #endregion
    }

    #region User interface variables
    public SliderType[] sliderTypes;
    public float[] sliderValues;

    public ToggleType[] toggleTypes;
    public bool[] toggleValues;

    public string[] scoreLeadBoardNames;
    public int[] scoreLeaderBoardValues;

    public string[] waveLeadBoardNames;
    public int[] waveLeaderBoardValues;
    #endregion

    public bool SkipTutorial;
}
