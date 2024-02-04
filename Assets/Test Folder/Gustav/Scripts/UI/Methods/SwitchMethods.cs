using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SwitchMethods : MonoBehaviour
{
    public SwitchType switchType;
    public void SaveToDataManager(DataManager manager, bool value)
    {
        if (!manager.switchValues.ContainsKey(switchType))
        {
            manager.switchValues.Add(switchType, value);
        }
        else
        {
            manager.switchValues[switchType] = value;
        }
    }
}
