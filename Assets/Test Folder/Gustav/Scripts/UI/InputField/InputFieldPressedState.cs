using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldPressedState : InputFieldBaseState
{
    private Color newTextColor = new(1, 1, 0, 0.8f);
    private float timeItTakes = 0.2f;

    public override void EnterState(InputFieldStateManager inputField)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(inputField.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(InputFieldStateManager inputField)
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (inputField.text.text.Length != 0)
                {
                    inputField.text.text = inputField.text.text.Substring(0, inputField.text.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                inputField.SwitchState(inputField.deselectedState);
                Debug.Log("User entered: " + inputField.text.text);

                break;
            }
            else
            {
                inputField.text.text += c;
            }
        }
    }

    public override void ExitState(InputFieldStateManager inputField)
    {

    }
}
