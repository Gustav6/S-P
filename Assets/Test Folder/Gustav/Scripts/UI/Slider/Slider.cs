using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : UI
{
    private SliderStateManager SliderStateManager { get; set; }

    [Header("Slider variables")]
    public SliderType sliderType;

    public override void Start()
    {
        SliderStateManager = GetComponent<SliderStateManager>();

        LoadFunction(SliderStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(SliderStateManager);

        base.Update();
    }
    public void SaveToDataManager(UIDataManager manager, float value, SliderType type)
    {
        if (manager.sliderValues.ContainsKey(type))
        {
            manager.sliderValues[type] = value;

            for (int i = 0; i < UIDataManager.instance.Currentdata.sliderTypes.Length; i++)
            {
                if (type == (SliderType)i)
                {
                    UIDataManager.instance.Currentdata.sliderValues[i] = value;
                }
            }
        }

        SaveSystem.Instance.SaveData(UIDataManager.instance.Currentdata);
    }
}
