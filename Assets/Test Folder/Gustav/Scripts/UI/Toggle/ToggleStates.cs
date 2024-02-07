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
        manager = referenceManager.ToggleManagerInstance;

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

public class ToggleSelectedState : UIBaseState
{
    private ToggleStateManager manager;

    public float timeItTakes = 0.2f;
    Color newMovingPartColor = new(0, 0, 0, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = referenceManager.ToggleManagerInstance;

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
            if (!manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
            {
                manager.SwitchState(manager.deselectedState);
            }
        }

        if (manager.UIActivated)
        {
            manager.switchOn = !manager.switchOn;
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
    float timer;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = referenceManager.ToggleManagerInstance;

        timer = manager.transitionTime;
        TransitionFromOnOff(manager, timer);
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (timer <= 0)
        {
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
                if (manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
                {
                    manager.SwitchState(manager.selectedState);
                }
                else
                {
                    manager.SwitchState(manager.deselectedState);
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
        //SaveToDataManager(UIManager.DataManagerInstance, @switch.switchOn);
    }

    public void TransitionFromOnOff(ToggleStateManager toggle, float transitionTime)
    {
        if (toggle.switchOn)
        {
            Vector3 destination = new(toggle.movingPartOffset * -1 * UIManager.ResolutionScaling, 0, 0);
            destination += toggle.outLine.position;
            TransitionSystem.AddMoveTransition(new MoveTransition(toggle.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(toggle.toggleImage, toggle.onColor, transitionTime, TransitionType.SmoothStop2));
        }
        else
        {
            Vector3 destination = new(toggle.movingPartOffset * UIManager.ResolutionScaling, 0, 0);
            destination += toggle.outLine.position;
            TransitionSystem.AddMoveTransition(new MoveTransition(toggle.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(toggle.toggleImage, toggle.offColor, transitionTime, TransitionType.SmoothStop2));
        }

        manager.UIActivated = false;
    }
}
