using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDeselectedState : UIBaseState
{
    private SliderStateManager manager;
    private Slider sliderInstance;

    private readonly float timeItTakes = 0.2f;
    Color newInterPartColor = new(1, 1, 1, 0.5f);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, manager.transform, manager.outLineImage, manager.text);

        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newInterPartColor, timeItTakes, TransitionType.SmoothStart2));
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

    private Vector3 newScale = new(1.1f, 1.1f, 1);
    private Color newSliderColor = new(1, 1, 1, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        if (!UIManager.instance.Transitioning)
        {
            manager.DefaultSelectTransition(timeItTakes, manager.pointers, manager.transform, manager.outLineImage, manager.text);

            TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newSliderColor, timeItTakes, TransitionType.SmoothStart2));
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
    Color newSlidingPartColor = new(1, 1, 0, 1);
    Color originalColor;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (SliderStateManager)referenceManager;
        sliderInstance = (Slider)referenceManager.UIInstance;

        originalColor = manager.sliderImage.color;
        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newSlidingPartColor, timeItTakes, TransitionType.SmoothStop2));
        UIManager.instance.ChangingSlider = true;
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (UIManager.instance.KeyOrControlActive)
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
                MoveTowardsMouse(manager, UIManager.instance.MousePosition.x);
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
        UIManager.instance.ChangingSlider = false;
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
        float scaling = UIManager.instance.ResolutionScaling;

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
        float scaling = UIManager.instance.ResolutionScaling;
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

        slider.sliderPosition.localPosition += des;
    }
}