using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDeselectedState : UIBaseState
{
    private InputFieldStateManager manager;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;

        //inputField.pointers.SetActive(false);
        //TransitionSystem.AddColorTransition(new ColorTransition(inputField.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.UIManagerInstance.CurrentUISelected == manager.UIInstance.position)
            {
                manager.SwitchState(manager.selectedState);
            }
        }
        else
        {
            if (manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
            {
                manager.SwitchState(manager.selectedState);
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}
public class InputFieldSelectedState : UIBaseState
{
    private InputFieldStateManager manager;

    private float timer;
    private Color newTextColor = new(1, 1, 1, 1);
    private float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (!UIManager.Transitioning)
        {
            manager = (InputFieldStateManager)referenceManager;

            timer = timeItTakes;
            TransitionSystem.AddColorTransition(new ColorTransition(manager.text, newTextColor, timeItTakes, TransitionType.SmoothStart2));
            manager.pointers.SetActive(true);
        }
        else
        {
            manager.SwitchState(manager.deselectedState);
        }
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (manager.UIManagerInstance.KeyOrControlActive)
        {
            if (manager.UIManagerInstance.CurrentUISelected != manager.UIInstance.position)
            {
                manager.SwitchState(manager.deselectedState);
            }
        }
        else
        {
            if (!manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
            {
                manager.SwitchState(manager.deselectedState);
            }
        }

        if (timer <= 0)
        {
            if (manager.UIActivated)
            {
                manager.SwitchState(manager.pressedState);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}

public class InputFieldPressedState : UIBaseState
{
    private InputFieldStateManager manager;
    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;
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
                manager.SwitchState(manager.deselectedState);
                Debug.Log("User entered: " + manager.text.text);

                break;
            }
            else
            {
                manager.text.text += c;
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}
