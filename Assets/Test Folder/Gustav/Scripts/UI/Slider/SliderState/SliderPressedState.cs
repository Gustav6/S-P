using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderPressedState : SliderBaseState
{
    readonly float timeItTakes = 0.15f;
    Color newSlidingPartColor = new(1, 1, 0, 1);
    Color originalColor;

    public override void EnterState(SliderStateManager slider)
    {
        originalColor = slider.sliderImage.color;
        //slider.uI.AudioManagerInstance.PlaySound(AudioType.ClickSound);
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
            }
        }
        else
        {
            if (slider.uI.UIManagerInstance.HoveringGameObject(slider.gameObject) && slider.uI.activated)
            {
                MoveTowardsMouse(slider, slider.uI.UIManagerInstance.MousePosition.x);
            }

            if (!slider.uI.UIManagerInstance.HoveringGameObject(slider.gameObject))
            {
                slider.uI.activated = false;
                slider.SwitchState(slider.deselectedState);
            }
        }

        if (!slider.uI.activated)
        {
            slider.SwitchState(slider.selectedState);
        }
    }

    public override void ExitState(SliderStateManager slider)
    {
        ApplyValues(slider);
        ResetButtonValue(slider);
        slider.uI.UIManagerInstance.ChangingSlider = false;
    }

    private void ApplyValues(SliderStateManager slider)
    {
        slider.methods.SaveToDataManager(UIManager.DataManagerInstance, slider.TotalSlidingPercentage());
    }

    private void ResetButtonValue(SliderStateManager slider)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, originalColor, timeItTakes, TransitionType.SmoothStop2));
    }

    private void MoveTowardsMouse(SliderStateManager slider, float mouseX)
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
        float moveAmount = slider.maxMoveValue / 200;

        Vector3 des = new(moveAmount * slider.moveDirection * scaling, 0, 0);

        if (slider.sliderPosition.localPosition.x * scaling + des.x > slider.maxMoveValue * scaling)
        {
            des.x = (slider.maxMoveValue * scaling) - slider.sliderPosition.localPosition.x * scaling;
        }
        else if (slider.sliderPosition.localPosition.x * scaling + des.x < -slider.maxMoveValue * scaling)
        {
            des.x = (-slider.maxMoveValue * scaling) - slider.sliderPosition.localPosition.x * scaling;
        }

        slider.sliderPosition.localPosition += des;
    }
}
