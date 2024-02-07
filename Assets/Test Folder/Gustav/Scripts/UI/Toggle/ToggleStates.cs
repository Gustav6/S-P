using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDeselectedState : UIBaseState
{
    private ToggleStateManager manager;

    public float timeItTakes = 0.25f;
    Color newMovingPartColor = new(0, 0, 0, 0.2f);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, null, manager.outLineImage, manager.text);

        TransitionSystem.AddColorTransition(new ColorTransition(manager.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
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
            if (manager.Hovering(manager.UIInstance, manager.UIManagerInstance))
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

public class ToggleSelectedState : UIBaseState
{
    private ToggleStateManager manager;
    private Toggle toggleInstance;

    public float timeItTakes = 0.2f;
    Color newMovingPartColor = new(0, 0, 0, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (!UIManager.Transitioning)
        {
            manager.DefaultSelectTransition(timeItTakes, manager.pointers, null, manager.outLineImage, manager.text);

            TransitionSystem.AddColorTransition(new ColorTransition(manager.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
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
            if (!manager.Hovering(manager.UIInstance, manager.UIManagerInstance))
            {
                manager.SwitchState(manager.deselectedState);
            }
        }

        if (manager.UIActivated)
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
        referenceManager.StartCoroutine(WaitCoroutine(toggleInstance.transitionTime));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {

    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        toggleInstance.SaveToDataManager(UIManager.DataManagerInstance, toggleInstance.toggleOn, toggleInstance.toggleType);
    }

    public void TransitionFromOnOff(Toggle toggle, ToggleStateManager manager, float transitionTime)
    {
        if (toggle.toggleOn)
        {
            Vector3 destination = new(manager.movingPartOffset * -1 * UIManager.ResolutionScaling, 0, 0);
            destination += manager.outLine.position;
            TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(manager.toggleImage, toggle.onColor, transitionTime, TransitionType.SmoothStop2));
        }
        else
        {
            Vector3 destination = new(manager.movingPartOffset * UIManager.ResolutionScaling, 0, 0);
            destination += manager.outLine.position;
            TransitionSystem.AddMoveTransition(new MoveTransition(manager.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(manager.toggleImage, toggle.offColor, transitionTime, TransitionType.SmoothStop2));
        }

        manager.UIActivated = false;
    }
    public IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.UIManagerInstance.currentUISelected == manager.UIInstance.position)
            {
                manager.SwitchState(manager.selectedState);
            }
            else
            {
                manager.SwitchState(manager.deselectedState);
            }
        }
        else
        {
            if (manager.Hovering(manager.UIInstance, manager.UIManagerInstance))
            {
                manager.SwitchState(manager.selectedState);
            }
            else
            {
                manager.SwitchState(manager.deselectedState);
            }
        }
    }
}
