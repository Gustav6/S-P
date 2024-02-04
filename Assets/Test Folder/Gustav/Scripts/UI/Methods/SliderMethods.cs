using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMethods : MonoBehaviour
{   
    public SliderType type;

    public void SaveToDataManager(DataManager manager, float value)
    {
        if (!manager.sliderValues.ContainsKey(type))
        {
            manager.sliderValues.Add(type, value);
        }
        else
        {
            manager.sliderValues[type] = value;
        }
    }
}
