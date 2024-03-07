using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Deselected State
public class ToggleDeselectedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (toggleInstance.version == ToggleVersion.Version1)
        {
            manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, null, manager.outLineImage, null);
        }
        else if (toggleInstance.version == ToggleVersion.Version2)
        {
            manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, null, manager.outLineImage, manager.textForV2);
        }
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(manager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Hover");
        }
    }
}
#endregion

#region Selected State
public class ToggleSelectedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    private readonly float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (!UIStateManager.Instance.Transitioning)
        {
            if (toggleInstance.version == ToggleVersion.Version1)
            {
                manager.DefaultSelectTransition(timeItTakes, manager.Pointers, null, manager.outLineImage, null);
            }
            else if (toggleInstance.version == ToggleVersion.Version2)
            {
                manager.DefaultSelectTransition(timeItTakes, manager.Pointers, null, manager.outLineImage, manager.textForV2);
            }
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
#endregion

#region Pressed State
public class TogglePressedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Click");
        }

        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (toggleInstance.version == ToggleVersion.Version1)
        {
            manager.StartCoroutine(WaitCoroutine(toggleInstance.transitionTime));
        }

        TransitionFromOnOff(toggleInstance, manager, toggleInstance.transitionTime);
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (toggleInstance.version == ToggleVersion.Version1)
        {
            if (canTransition)
            {
                CheckIfDeselected(manager, manager.deselectedState);
                CheckIfSelected(manager, manager.selectedState);
            }
        }
        else if (toggleInstance.version == ToggleVersion.Version2)
        {
            CheckIfDeselected(manager, manager.deselectedState);
            CheckIfSelected(manager, manager.selectedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (toggleInstance.version == ToggleVersion.Version1)
        {
            toggleInstance.SaveToDataManager(UIDataManager.instance, toggleInstance.toggleOn, toggleInstance.toggleType);
        }
        else if (toggleInstance.version == ToggleVersion.Version2)
        {
            toggleInstance.ActivateSelectedFunctions();
        }
    }

    public void TransitionFromOnOff(Toggle toggle, ToggleStateManager manager, float transitionTime)
    {
        if (toggleInstance.version == ToggleVersion.Version1)
        {
            if (toggle.toggleOn)
            {
                Vector3 destination = new(manager.movingPartOffset * -2 * UIStateManager.Instance.ResolutionScaling, 0, 0);
                TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3, true));
            }
            else
            {
                Vector3 destination = new(manager.movingPartOffset * 2 * UIStateManager.Instance.ResolutionScaling, 0, 0);
                TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3, true));
            }
        }
        else if (toggleInstance.version == ToggleVersion.Version2)
        {
            if (toggle.toggleOn)
            {
                manager.checkMark.SetActive(true);
            }
            else
            {
                manager.checkMark.SetActive(false);
            }
        }

        manager.UIActivated = false;
    }
}
#endregion