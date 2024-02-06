using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldDeselectedState : InputFieldBaseState
{
    private Color newTextColor = new(1, 1, 1, 0.6f);
    private float timeItTakes = 0.2f;

    public override void EnterState(InputFieldStateManager inputField)
    {
        inputField.pointers.SetActive(false);
        TransitionSystem.AddColorTransition(new ColorTransition(inputField.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(InputFieldStateManager inputField)
    {
        if (inputField.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (inputField.uI.UIManagerInstance.CurrentUISelected == inputField.uI.position)
            {
                inputField.SwitchState(inputField.selectedState);
            }
        }
        else
        {
            if (inputField.uI.UIManagerInstance.HoveringGameObject(inputField.gameObject))
            {
                inputField.SwitchState(inputField.selectedState);
            }
        }
    }

    public override void ExitState(InputFieldStateManager inputField)
    {
        //inputField.uI.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
