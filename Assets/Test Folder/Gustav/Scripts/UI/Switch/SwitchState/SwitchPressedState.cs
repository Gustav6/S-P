using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SwitchPressedState : SwitchBaseState
{
    float timer = 0.3f;

    public override void EnterState(SwitchStateManager @switch)
    {
        timer = @switch.transitionTime;
        TransitionFromOnOff(@switch, timer);
    }

    public override void UpdateState(SwitchStateManager @switch)
    {
        if (timer <= 0)
        {
            if (@switch.uI.UIManagerInstance.KeyOrControlActive)
            {
                if (@switch.uI.UIManagerInstance.currentUISelected == @switch.uI.position)
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
                if (@switch.uI.UIManagerInstance.HoveringGameObject(@switch.gameObject))
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
        @switch.methods.SaveToDataManager(UIManager.DataManagerInstance, @switch.switchOn);
    }

    public void TransitionFromOnOff(SwitchStateManager @switch, float transitionTime)
    {
        if (@switch.switchOn)
        {
            Vector3 destination = new(@switch.movingPartOffset * -1 * UIManager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0, 0.8f, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, @switch.onColor, transitionTime, TransitionType.SmoothStop2));
        }
        else
        {
            Vector3 destination = new(@switch.movingPartOffset * UIManager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, @switch.offColor, transitionTime, TransitionType.SmoothStop2));
        }

        @switch.uI.activated = false;
    }
}
