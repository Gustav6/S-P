using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDeselectedState : UIBaseState
{
    private SliderStateManager manager;
    private Slider sliderInstance;

    private readonly float timeItTakes = 0.2f;
    private readonly Color newSliderColor = new(1, 1, 1, 0.9f);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, manager.transform, manager.outLineImage, manager.text);

        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newSliderColor, timeItTakes, TransitionType.SmoothStart2));
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(referenceManager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        manager.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
public class SliderSelectedState : UIBaseState
{
    private SliderStateManager manager;
    private Slider sliderInstance;

    private readonly float timeItTakes = 0.15f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        if (!UIManager.Instance.Transitioning)
        {
            manager.DefaultSelectTransition(timeItTakes, manager.pointers, manager.transform, manager.outLineImage, manager.text);
        }
        else
        {
            manager.SwitchState(manager.deselectedState);
        }
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfDeselected(referenceManager, manager.deselectedState);

        if (manager.UIActivated)
        {
            manager.SwitchState(manager.pressedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}

public class SliderPressedState : UIBaseState
{
    private SliderStateManager manager;
    private Slider sliderInstance;

    readonly float timeItTakes = 0.15f;
    private readonly Color newSliderColor = new(1, 1, 1, 1);
    Color originalColor;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        originalColor = manager.sliderImage.color;
        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newSliderColor, timeItTakes, TransitionType.SmoothStop2));
        UIManager.Instance.ChangingSlider = true;
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (UIManager.Instance.KeyOrControlActive)
        {
            if (manager.moveDirection != 0)
            {
                MoveWithButton(manager);
            }
        }
        else
        {
            if (referenceManager.UIInstance.hovering && manager.UIActivated)
            {
                MoveTowardsMouse(manager, UIManager.Instance.MousePosition.x);
            }

            if (!referenceManager.UIInstance.hovering)
            {
                manager.UIActivated = false;
                manager.SwitchState(manager.deselectedState);
            }
        }

        if (!manager.UIActivated)
        {
            manager.SwitchState(manager.selectedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        ApplyValues(manager);
        ResetButtonValue(manager);
        UIManager.Instance.ChangingSlider = false;
    }
    private void ApplyValues(SliderStateManager slider)
    {
        sliderInstance.SaveToDataManager(UIDataManager.instance, slider.TotalSlidingPercentage(), sliderInstance.sliderType);
    }

    private void ResetButtonValue(SliderStateManager slider)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(slider.sliderImage, originalColor, timeItTakes, TransitionType.SmoothStop2));
    }

    private void MoveTowardsMouse(SliderStateManager slider, float mouseX)
    {
        float scaling = UIManager.Instance.ResolutionScaling;

        if (slider.sliderPosition.localPosition.x * scaling > -slider.maxMoveValue * scaling || slider.sliderPosition.localPosition.x * scaling < slider.maxMoveValue * scaling)
        {
            Vector3 mousePosition = new(mouseX, slider.sliderPosition.position.y, slider.sliderPosition.position.z);

            if (mousePosition.x >= (slider.maxMoveValue * scaling) + slider.outLinePosition.position.x)
            {
                Vector3 destination = new(slider.maxMoveValue, 0, 0);
                slider.sliderPosition.localPosition = destination;
            }
            else if (mousePosition.x <= (-slider.maxMoveValue * scaling) + slider.outLinePosition.position.x)
            {
                Vector3 destination = new(-slider.maxMoveValue, 0, 0);
                slider.sliderPosition.localPosition = destination;
            }
            else
            {
                slider.sliderPosition.position = mousePosition;
            }
        }
    }

    public void MoveWithButton(SliderStateManager slider)
    {
        float scaling = UIManager.Instance.ResolutionScaling;
        float moveAmount = slider.maxMoveValue / (100 * scaling);
        Vector3 limit = new(slider.maxMoveValue, 0, 0);

        Vector3 des = new(moveAmount * slider.moveDirection * scaling, 0, 0);

        if (slider.sliderPosition.localPosition.x * scaling + des.x > slider.maxMoveValue * scaling)
        {
            slider.sliderPosition.localPosition = limit;
        }
        else if (slider.sliderPosition.localPosition.x * scaling + des.x < -slider.maxMoveValue * scaling)
        {
            slider.sliderPosition.localPosition = -limit;
        }
        else
        {
            slider.sliderPosition.localPosition += des;
        }
    }
}
