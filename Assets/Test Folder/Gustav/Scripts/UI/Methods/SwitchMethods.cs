using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SwitchMethods : MonoBehaviour
{
    public SwitchType type;
    public void SaveToDataManager(DataManager manager, bool value)
    {
        if (!manager.switchValues.ContainsKey(type))
        {
            manager.switchValues.Add(type, value);
        }
        else
        {
            manager.switchValues[type] = value;
        }
    }
}
