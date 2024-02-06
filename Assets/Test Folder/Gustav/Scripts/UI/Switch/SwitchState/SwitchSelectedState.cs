using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSelectedState : SwitchBaseState
{
    public float timeItTakes = 0.2f;

    Color newOutlineColor = new(0, 0, 0, 1);
    Color newMovingPartColor = new(0, 0, 0, 1);
    Color newTextColor = new(1, 1, 0, 0.8f);


    public override void EnterState(SwitchStateManager @switch)
    {
        if (!UIManager.Transitioning)
        {
            @switch.pointers.SetActive(true);

            TransitionSystem.AddColorTransition(new ColorTransition(@switch.outLineImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
        }
        else
        {
            @switch.SwitchState(@switch.deselectedState);
        }
    }

    public override void UpdateState(SwitchStateManager @switch)
    {
        if (@switch.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (@switch.uI.UIManagerInstance.CurrentUISelected != @switch.uI.position)
            {
                @switch.SwitchState(@switch.deselectedState);
            }
        }
        else
        {
            if (!@switch.uI.UIManagerInstance.HoveringGameObject(@switch.gameObject))
            {
                @switch.SwitchState(@switch.deselectedState);
            }
        }

        if (@switch.uI.activated)
        {
            @switch.switchOn = !@switch.switchOn;
            @switch.SwitchState(@switch.pressedState);
        }
    }

    public override void ExitState(SwitchStateManager @switch)
    {

    }
}