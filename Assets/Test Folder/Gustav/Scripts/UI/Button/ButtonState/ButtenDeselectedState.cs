using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtenDeselectedState : ButtonBaseState
{
    private float timeItTakes = 0.25f;

    public override void EnterState(ButtonStateManager button)
    {
        button.uI.DefaultDeselectTransition(timeItTakes, button.pointers, button.transform, button.outlineImage, button.text);
    }

    public override void UpdateState(ButtonStateManager button)
    {
        if (button.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (button.uI.UIManagerInstance.CurrentUISelected == button.uI.position)
            {
                button.SwitchState(button.selectedState);
            }
        }
        else
        {
            if (button.uI.UIManagerInstance.HoveringGameObject(button.gameObject))
            {
                button.SwitchState(button.selectedState);
            }
        }
    }
    public override void ExitState(ButtonStateManager button)
    {
        button.uI.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
