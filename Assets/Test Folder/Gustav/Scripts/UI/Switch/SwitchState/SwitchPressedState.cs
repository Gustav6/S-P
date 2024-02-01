using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPressedState : SwitchBaseState
{
    float timer = 0.3f;

    public override void EnterState(SwitchStateManager @switch)
    {
        timer = @switch.transitionTime;
        @switch.SwitchOnOff(@switch, timer);
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

    }
}
