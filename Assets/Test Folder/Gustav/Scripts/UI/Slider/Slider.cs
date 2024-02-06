using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : UI
{
    public SliderStateManager SliderStateManager { get; private set; }

    public override void Start()
    {
        SliderStateManager = GetComponent<SliderStateManager>();
        SliderStateManager.UIInstance = this;

        if (SliderStateManager.UIInstance != null)
        {
            SliderStateManager.OnStart();
        }

        base.Start();
    }

    public override void Update()
    {
        if (SliderStateManager.UIInstance != null)
        {
            SliderStateManager.OnUpdate();
        }

        base.Update();
    }
}
