using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtenSelectedState : ButtonBaseState
{
    private float timer;
    private float timeItTakes = 0.2f;

    public override void EnterState(ButtonStateManager button)
    {
        if (!UIManager.Transitioning)
        {
            button.uI.DefaultSelectTransition(timeItTakes, button.pointers, button.transform, button.outlineImage, button.text);
        }
        else
        {
            button.SwitchState(button.deselectedState);
        }
    }

    public override void UpdateState(ButtonStateManager button)
    {
        if (button.uI.UIManagerInstance.KeyOrControlActive)
        {
            if (button.uI.UIManagerInstance.CurrentUISelected != button.uI.position)
            {
                button.SwitchState(button.deselectedState);
            }
        }
        else
        {
            if (!button.uI.UIManagerInstance.HoveringGameObject(button.gameObject))
            {
                button.SwitchState(button.deselectedState);
            }
        }

        if (timer <= 0)
        {
            if (button.uI.activated)
            {
                button.SwitchState(button.pressedState);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(ButtonStateManager button)
    {

    }
}
