using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDeselectedState : UIBaseState
{
    private InputFieldStateManager manager;
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, manager.transform, manager.outlineImage, manager.text);
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(referenceManager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}
public class InputFieldSelectedState : UIBaseState
{
    private InputFieldStateManager manager;

    private readonly Color textColor = new(1, 1, 1, 1);
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (!UIManager.Instance.Transitioning)
        {
            manager = (InputFieldStateManager)referenceManager;

            referenceManager.StartCoroutine(WaitCoroutine(timeItTakes));
            manager.DefaultSelectTransition(timeItTakes, manager.pointers, manager.transform, manager.outlineImage, null);

            TransitionSystem.AddColorTransition(new ColorTransition(manager.text, textColor, timeItTakes, TransitionType.SmoothStop2));
        }
        else
        {
            manager.SwitchState(manager.deselectedState);
        }
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfDeselected(referenceManager, manager.deselectedState);

        if (canTransition && HasPressed(manager))
        {
            manager.SwitchState(manager.pressedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}

public class InputFieldPressedState : UIBaseState
{
    private InputFieldStateManager manager;
    private InputField inputFieldInstance;

    private readonly Color textColor = new(1, 1, 0, 0.8f);
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;
        inputFieldInstance = (InputField)referenceManager.UIInstance;
        TransitionSystem.AddColorTransition(new ColorTransition(manager.text, textColor, timeItTakes, TransitionType.SmoothStop2));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (manager.text.text.Length != 0)
                {
                    manager.text.text = manager.text.text.Substring(0, manager.text.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                CheckIfDeselected(referenceManager, manager.deselectedState);
                CheckIfSelected(referenceManager, manager.selectedState);
                Debug.Log("User entered: " + manager.text.text);

                break;
            }
            else if (manager.text.text.Length < inputFieldInstance.maxAmountOfLetters)
            {
                manager.text.text += c;
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}
