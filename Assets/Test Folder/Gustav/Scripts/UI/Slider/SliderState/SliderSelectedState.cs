using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSelectedState : SliderBaseState
{
    public float timeItTakes = 0.15f;
    public Vector3 newScale = new(1.1f, 1.1f, 1);

    Color newSliderColor = new(1, 1, 1, 1);

    public override void EnterState(SliderStateManager slider)
    {
        if (!UIManager.Transitioning)
        {
            slider.uI.DefaultSelectTransition(timeItTakes, slider.pointers, slider.transform, slider.outLineImage, slider.text);

            TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, newSliderColor, timeItTakes, TransitionType.SmoothStart2));
        }
        else
        {
            slider.SwitchState(slider.deselectedState);
        }
    }

    public override void UpdateState(SliderStateManager slider)
    {
        if (slider.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (slider.uI.UIManagerInstance.CurrentUISelected != slider.uI.position)
            {
                slider.SwitchState(slider.deselectedState);
            }
        }
        else
        {
            if (!slider.uI.UIManagerInstance.HoveringGameObject(slider.gameObject))
            {
                slider.SwitchState(slider.deselectedState);
            }
        }

        if (slider.uI.activated)
        {
            slider.SwitchState(slider.pressedState);
        }
    }

    public override void ExitState(SliderStateManager slider)
    {

    }
}
