using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMethods : MonoBehaviour
{   
    public SliderType sliderType;

    public void SaveToDataManager(DataManager manager, float value)
    {
        if (!manager.sliderValues.ContainsKey(sliderType))
        {
            manager.sliderValues.Add(sliderType, value);
        }
        else
        {
            manager.sliderValues[sliderType] = value;
        }
    }
}
