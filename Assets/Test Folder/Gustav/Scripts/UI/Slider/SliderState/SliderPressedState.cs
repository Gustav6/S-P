using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliderPressedState : SliderBaseState
{
    readonly float timeItTakes = 0.15f;
    readonly float moveAmount = 50;
    Color newSlidingPartColor = new(1, 1, 0, 1);
    Color originalColor;

    public override void EnterState(SliderStateManager slider)
    {
        originalColor = slider.sliderImage.color;
        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, newSlidingPartColor, timeItTakes, TransitionType.SmoothStop2));
        slider.uI.UIManagerInstance.ChangingSlider = true;
    }

    public override void UpdateState(SliderStateManager slider)
    {
        if (slider.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (slider.moveDirection != 0)
            {
                MoveWithButton(slider);
                slider.moveDirection = 0;
            }
        }
        else
        {
            if (slider.uI.activated)
            {
                MoveTowardsMouse(slider, slider.uI.UIManagerInstance.MousePosition.x);
            }

            if (!slider.uI.UIManagerInstance.HoveringGameObject(slider.gameObject))
            {
                slider.SwitchState(slider.deselectedState);
                slider.uI.activated = false;
            }
        }

        if (!slider.uI.activated)
        {
            slider.SwitchState(slider.selectedState);
        }
    }

    public override void ExitState(SliderStateManager slider)
    {
        ResetButtonValue(slider);
        slider.methods.SaveToDataManager(UIManager.DataManagerInstance, slider.TotalSlidingPercentage());
        slider.uI.UIManagerInstance.ChangingSlider = false;
    }

    public void ResetButtonValue(SliderStateManager slider)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, originalColor, timeItTakes, TransitionType.SmoothStop2));
    }

    public void MoveTowardsMouse(SliderStateManager slider, float mouseX)
    {
        float scaling = UIManager.ResolutionScaling;

        if (slider.sliderPosition.localPosition.x * scaling > -slider.maxMoveValue * scaling || slider.sliderPosition.localPosition.x * scaling < slider.maxMoveValue * scaling)
        {
            Vector3 des = new(mouseX, slider.sliderPosition.position.y, slider.sliderPosition.position.z);

            if (des.x > slider.maxMoveValue * scaling + slider.outLineImage.transform.position.x)
            {
                des.x = (slider.maxMoveValue * scaling) + slider.outLineImage.transform.position.x;
            }
            else if (des.x < -slider.maxMoveValue * scaling + slider.outLineImage.transform.position.x)
            {
                des.x = (-slider.maxMoveValue * scaling) + slider.outLineImage.transform.position.x;
            }

            slider.sliderPosition.position = des;
        }
    }

    public void MoveWithButton(SliderStateManager slider)
    {
        float scaling = UIManager.ResolutionScaling;

        Vector3 des = new(moveAmount * slider.moveDirection * scaling, 0, 0);
        float timeItTakes = 0.1f;

        if (slider.sliderPosition.localPosition.x * scaling + des.x > slider.maxMoveValue * scaling)
        {
            des.x = (slider.maxMoveValue * scaling) - slider.sliderPosition.localPosition.x * scaling;
        }
        else if (slider.sliderPosition.localPosition.x * scaling + des.x < -slider.maxMoveValue * scaling)
        {
            des.x = (-slider.maxMoveValue * scaling) - slider.sliderPosition.localPosition.x * scaling;
        }

        TransitionSystem.AddMoveTransition(new MoveTransition(slider.sliderPosition.transform, des, timeItTakes, TransitionType.SmoothStop2, true));
    }
}
