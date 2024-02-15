using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDeselectedState : UIBaseState
{
    private ToggleStateManager manager;

    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, null, manager.outLineImage, null);
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(manager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        manager.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}

public class ToggleSelectedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    private readonly float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (!UIManager.Instance.Transitioning)
        {
            manager.DefaultSelectTransition(timeItTakes, manager.pointers, null, manager.outLineImage, null);
        }
        else
        {
            manager.SwitchState(manager.deselectedState);
        }
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfDeselected(manager, manager.deselectedState);

        if (HasPressed(manager))
        {
            toggleInstance.toggleOn = !toggleInstance.toggleOn;
            manager.SwitchState(manager.pressedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}

public class TogglePressedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        TransitionFromOnOff(toggleInstance, manager, toggleInstance.transitionTime);
        manager.StartCoroutine(WaitCoroutine(toggleInstance.transitionTime));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (canTransition)
        {
            CheckIfDeselected(manager, manager.deselectedState);
            CheckIfSelected(manager, manager.selectedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        toggleInstance.SaveToDataManager(UIDataManager.instance, toggleInstance.toggleOn, toggleInstance.toggleType);
    }

    public void TransitionFromOnOff(Toggle toggle, ToggleStateManager manager, float transitionTime)
    {
        if (toggle.toggleOn)
        {
            Vector3 destination = new(manager.movingPartOffset * -2 * UIManager.Instance.ResolutionScaling, 0, 0);
            TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3, true));
        }
        else
        {
            Vector3 destination = new(manager.movingPartOffset * 2 * UIManager.Instance.ResolutionScaling, 0, 0);
            TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3, true));
        }

        manager.UIActivated = false;
    }
}
