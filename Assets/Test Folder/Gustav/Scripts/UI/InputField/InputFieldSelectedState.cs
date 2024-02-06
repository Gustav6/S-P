using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldSelectedState : InputFieldBaseState
{
    private float timer;
    private Color newTextColor = new(1, 1, 1, 1);
    private float timeItTakes = 0.25f;

    public override void EnterState(InputFieldStateManager inputField)
    {
        if (!UIManager.Transitioning)
        {
            timer = timeItTakes;
            TransitionSystem.AddColorTransition(new ColorTransition(inputField.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
            inputField.pointers.SetActive(true);
        }
        else
        {
            inputField.SwitchState(inputField.deselectedState);
        }
    }

    public override void UpdateState(InputFieldStateManager inputField)
    {
        if (inputField.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (inputField.uI.UIManagerInstance.CurrentUISelected != inputField.uI.position)
            {
                inputField.SwitchState(inputField.deselectedState);
            }
        }
        else
        {
            if (!inputField.uI.UIManagerInstance.HoveringGameObject(inputField.gameObject))
            {
                inputField.SwitchState(inputField.deselectedState);
            }
        }

        if (timer <= 0)
        {
            if (inputField.uI.activated)
            {
                inputField.SwitchState(inputField.pressedState);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(InputFieldStateManager inputField)
    {

    }
}
