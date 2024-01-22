using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPressedState : SwitchBaseState
{
    float timer;
    float transitionTime = 0.3f;

    public override void EnterState(SwitchStateManager @switch)
    {
        timer = transitionTime;
        SwitchToOnOrOff(@switch);
    }

    public override void UpdateState(SwitchStateManager @switch)
    {
        if (timer <= 0)
        {
            if (@switch.uI.Manager.KeyOrControlActive)
            {
                if (@switch.uI.Manager.CurrentButten == @switch.gameObject)
                {
                    @switch.SwitchState(@switch.selectedState);
                }
                else
                {
                    @switch.SwitchState(@switch.deselectedState);
                }
            }
            else
            {
                if (@switch.uI.Manager.HoveringGameObject(@switch.gameObject))
                {
                    @switch.SwitchState(@switch.selectedState);
                }
                else
                {
                    @switch.SwitchState(@switch.deselectedState);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(SwitchStateManager @switch)
    {

    }

    public void SwitchToOnOrOff(SwitchStateManager @switch)
    {
        if (@switch.switchStatus)
        {
            Vector3 destination = new(@switch.movingPartOffset * -1 * @switch.uI.Manager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0, 1, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }
        else if (!@switch.switchStatus)
        {
            Vector3 destination = new(@switch.movingPartOffset * @switch.uI.Manager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(1, 0, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }

        @switch.uI.activated = false;
    }
}
