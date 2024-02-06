using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDeselectedState : SwitchBaseState
{
    public float timeItTakes = 0.25f;

    Color newMovingPartColor = new(0, 0, 0, 0.2f);

    public override void EnterState(SwitchStateManager @switch)
    {
        @switch.uI.DefaultDeselectTransition(timeItTakes, @switch.pointers, null, @switch.outLineImage, @switch.text);

        TransitionSystem.AddColorTransition(new ColorTransition(@switch.movingPartImage, newMovingPartColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(SwitchStateManager @switch)
    {
        if (@switch.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (@switch.uI.UIManagerInstance.CurrentUISelected == @switch.uI.position)
            {
                @switch.SwitchState(@switch.selectedState);
            }
        }
        else
        {
            if (@switch.uI.UIManagerInstance.HoveringGameObject(@switch.gameObject))
            {
                @switch.SwitchState(@switch.selectedState);
            }
        }
    }

    public override void ExitState(SwitchStateManager @switch)
    {
        @switch.uI.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
