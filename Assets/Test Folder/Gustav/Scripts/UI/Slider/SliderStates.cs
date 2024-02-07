using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDeselectedState : UIBaseState
{
    private SliderStateManager manager;

    public float timeItTakes = 0.2f;
    Color newInterPartColor = new(1, 1, 1, 0.5f);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = referenceManager.SliderManagerInstance;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, manager.transform, manager.outLineImage, manager.text);

        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newInterPartColor, timeItTakes, TransitionType.SmoothStart2));
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.UIManagerInstance.CurrentUISelected == manager.UIInstance.position)
            {
                manager.SwitchState(manager.selectedState);
            }
        }
        else
        {
            if (manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
            {
                manager.SwitchState(manager.selectedState);
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        manager.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
public class SliderSelectedState : UIBaseState
{
    private SliderStateManager manager;

    public float timeItTakes = 0.15f;
    public Vector3 newScale = new(1.1f, 1.1f, 1);

    Color newSliderColor = new(1, 1, 1, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = referenceManager.SliderManagerInstance;

        if (!UIManager.Transitioning)
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
        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.UIManagerInstance.CurrentUISelected != manager.UIInstance.position)
            {
                manager.SwitchState(manager.deselectedState);
            }
        }
        else
        {
            if (!manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
            {
                manager.SwitchState(manager.deselectedState);
            }
        }

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

    readonly float timeItTakes = 0.15f;
    Color newSlidingPartColor = new(1, 1, 0, 1);
    Color originalColor;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = referenceManager.SliderManagerInstance;

        originalColor = manager.sliderImage.color;
        TransitionSystem.AddColorTransition(new ColorTransition(manager.sliderImage, newSlidingPartColor, timeItTakes, TransitionType.SmoothStop2));
        manager.UIManagerInstance.ChangingSlider = true;
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.moveDirection != 0)
            {
                MoveWithButton(manager);
            }
        }
        else
        {
            if (manager.UIManagerInstance.HoveringGameObject(manager.gameObject) && manager.UIActivated)
            {
                MoveTowardsMouse(manager, manager.UIManagerInstance.MousePosition.x);
            }

            if (!manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
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
        manager.UIManagerInstance.ChangingSlider = false;
    }
    private void ApplyValues(SliderStateManager slider)
    {
        //slider.methods.SaveToDataManager(UIManager.DataManagerInstance, slider.TotalSlidingPercentage());
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
