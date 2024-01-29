using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDeselectedState : SliderBaseState
{
    public float timeItTakes = 0.2f;
    public Vector3 newScale = new(1, 1, 1);

    Color newOutlineColor = new(0, 0, 0, 0.5f);
    Color newInterPartColor = new(1, 1, 1, 0.5f);
    Color newTextColor = new(1, 1, 1, 0.5f);

    public override void EnterState(SliderStateManager slider)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(slider.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
        TransitionSystem.AddColorTransition(new ColorTransition(slider.outLineImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));
        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, newInterPartColor, timeItTakes, TransitionType.SmoothStart2));

        if (slider.uI.UIManagerInstance != null && !slider.uI.UIManagerInstance.Transitioning)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(slider.sliderImage.transform, newScale, timeItTakes, TransitionType.SmoothStart2));
        }
    }

    public override void UpdateState(SliderStateManager slider)
    {
        if (slider.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (slider.uI.UIManagerInstance.CurrentUISelected == slider.uI.position)
            {
                slider.uI.UIManagerInstance.CurrentUiElement = slider.gameObject;
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

    }
}