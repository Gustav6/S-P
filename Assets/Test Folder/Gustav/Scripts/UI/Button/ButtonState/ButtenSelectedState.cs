using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtenSelectedState : ButtonBaseState
{
    private float timer;
    private Vector3 newScale = new(1.025f, 1.025f, 1);
    private float timeItTakes = 0.2f;

    private Color newOutlineColor = new(0, 0, 0, 1);
    private Color newTextColor = new(1, 1, 0, 0.8f);

    public override void EnterState(ButtonStateManager button)
    {
        if (!UIManager.Transitioning)
        {
            button.pointers.gameObject.SetActive(true);

            TransitionSystem.AddScaleTransition(new ScaleTransition(button.transform, newScale, timeItTakes, TransitionType.SmoothStart2));
            TransitionSystem.AddColorTransition(new ColorTransition(button.image, newOutlineColor, timeItTakes, TransitionType.SmoothStop2));
            TransitionSystem.AddColorTransition(new ColorTransition(button.text, newTextColor, timeItTakes, TransitionType.SmoothStop2));
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
