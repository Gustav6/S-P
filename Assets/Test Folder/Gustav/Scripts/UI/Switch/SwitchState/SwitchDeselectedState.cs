using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDeselectedState : SwitchBaseState
{
    public float timeItTakes = 0.25f;

    Color newOutlineColor = new(0, 0, 0, 0.5f);
    Color newMovingPartColor = new(0, 0, 0, 0.2f);
    Color newTextColor = new(1, 1, 1, 0.5f);

    public override void EnterState(SwitchStateManager @switch)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(@switch.outLineImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));
        TransitionSystem.AddColorTransition(new ColorTransition(@switch.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
        TransitionSystem.AddColorTransition(new ColorTransition(@switch.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(SwitchStateManager @switch)
    {
        if (@switch.uI.Manager.KeyOrControlActive)
        {
            if (@switch.uI.Manager.CurrentUISelected == @switch.uI.position)
            {
                @switch.uI.Manager.CurrentUiElement = @switch.gameObject;
                @switch.SwitchState(@switch.selectedState);
            }
        }
        else
        {
            if (@switch.uI.Manager.HoveringGameObject(@switch.gameObject))
            {
                @switch.SwitchState(@switch.selectedState);
            }
        }
    }

    public override void ExitState(SwitchStateManager @switch)
    {

    }
}
