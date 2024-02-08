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

        leadBoardNames = new string[10];
        leaderBoardScore = new int[10];
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
        #endregion
    }

    #region User interface variables
    public SliderType[] sliderTypes;
    public float[] sliderValues;

    public ToggleType[] toggleTypes;
    public bool[] toggleValues;

    public string[] leadBoardNames;
    public int[] leaderBoardScore;
    #endregion
}
