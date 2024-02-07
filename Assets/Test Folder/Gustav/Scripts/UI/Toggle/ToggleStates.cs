using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDeselectedState : UIBaseState
{
    private ToggleStateManager managerInstance;

    public float timeItTakes = 0.25f;
    Color newMovingPartColor = new(0, 0, 0, 0.2f);

    public override void EnterState(BaseStateManager referenceManager)
    {
        managerInstance = (ToggleStateManager)referenceManager;

        managerInstance.DefaultDeselectTransition(timeItTakes, managerInstance.pointers, null, managerInstance.outLineImage, managerInstance.text);

        TransitionSystem.AddColorTransition(new ColorTransition(managerInstance.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (managerInstance.UIManagerInstance.KeyOrControlActive)
        {
            if (managerInstance.UIManagerInstance.CurrentUISelected == managerInstance.UIInstance.position)
            {
                managerInstance.SwitchState(managerInstance.selectedState);
            }
        }
        else
        {
            if (managerInstance.UIManagerInstance.HoveringGameObject(managerInstance.gameObject))
            {
                managerInstance.SwitchState(managerInstance.selectedState);
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        managerInstance.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}

public class ToggleSelectedState : UIBaseState
{
    private ToggleStateManager managerInstance;
    private Toggle toggleInstance;

    public float timeItTakes = 0.2f;
    Color newMovingPartColor = new(0, 0, 0, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        managerInstance = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        if (!UIManager.Transitioning)
        {
            managerInstance.DefaultSelectTransition(timeItTakes, managerInstance.pointers, null, managerInstance.outLineImage, managerInstance.text);

            TransitionSystem.AddColorTransition(new ColorTransition(managerInstance.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
        }
        else
        {
            managerInstance.SwitchState(managerInstance.deselectedState);
        }
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (managerInstance.UIManagerInstance.KeyOrControlActive)
        {
            if (managerInstance.UIManagerInstance.CurrentUISelected != managerInstance.UIInstance.position)
            {
                managerInstance.SwitchState(managerInstance.deselectedState);
            }
        }
        else
        {
            if (!managerInstance.UIManagerInstance.HoveringGameObject(managerInstance.gameObject))
            {
                managerInstance.SwitchState(managerInstance.deselectedState);
            }
        }

        if (managerInstance.UIActivated)
        {
            toggleInstance.toggleOn = !toggleInstance.toggleOn;
            managerInstance.SwitchState(managerInstance.pressedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}

public class TogglePressedState : UIBaseState
{
    private ToggleStateManager managerInstance;
    private Toggle toggleInstance;
    float timer;

    public override void EnterState(BaseStateManager referenceManager)
    {
        managerInstance = (ToggleStateManager)referenceManager;
        toggleInstance = (Toggle)referenceManager.UIInstance;

        timer = toggleInstance.transitionTime;
        TransitionFromOnOff(toggleInstance, managerInstance, timer);
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (timer <= 0)
        {
            if (managerInstance.UIManagerInstance.KeyOrControlActive)
            {
                if (managerInstance.UIManagerInstance.currentUISelected == managerInstance.UIInstance.position)
                {
                    managerInstance.SwitchState(managerInstance.selectedState);
                }
                else
                {
                    managerInstance.SwitchState(managerInstance.deselectedState);
                }
            }
            else
            {
                if (managerInstance.UIManagerInstance.HoveringGameObject(managerInstance.gameObject))
                {
                    managerInstance.SwitchState(managerInstance.selectedState);
                }
                else
                {
                    managerInstance.SwitchState(managerInstance.deselectedState);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
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
}
