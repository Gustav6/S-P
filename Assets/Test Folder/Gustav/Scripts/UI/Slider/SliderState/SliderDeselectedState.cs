using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDeselectedState : SliderBaseState
{
    public float timeItTakes = 0.2f;

    Color newInterPartColor = new(1, 1, 1, 0.5f);

    public override void EnterState(SliderStateManager slider)
    {
        slider.uI.DefaultDeselectTransition(timeItTakes, slider.pointers, slider.transform, slider.outLineImage, slider.text);

        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, newInterPartColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(SliderStateManager slider)
    {
        if (slider.uI.UIManagerInstance.KeyOrControlActive) 
        {
            if (slider.uI.UIManagerInstance.CurrentUISelected == slider.uI.position)
            {
                slider.SwitchState(slider.selectedState);
            }
        }
        else
        {
            if (slider.uI.UIManagerInstance.HoveringGameObject(slider.gameObject))
            {
                slider.SwitchState(slider.selectedState);
            }
        }
    }

    public override void ExitState(SliderStateManager slider)
    {
        slider.uI.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}