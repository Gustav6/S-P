using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : UI
{
    private SliderStateManager SliderStateManager { get; set; }

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
    public void SaveToDataManager(DataManager manager, float value, SliderType type)
    {
        if (manager.sliderValues.ContainsKey(type))
        {
            manager.sliderValues[type] = value;
        }
    }
}
