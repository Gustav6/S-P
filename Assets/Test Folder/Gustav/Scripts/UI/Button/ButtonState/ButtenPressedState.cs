using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtenPressedState : ButtonBaseState
{
    private float timer;
    private float timeItTakes = 0.15f;

    private Vector3 newScale = new(1, 1, 1);
    private Color newOutlineColor = new(1, 1, 1, 1);

    public override void EnterState(ButtonStateManager button)
    {
        timer = timeItTakes;
        TransitionSystem.AddScaleTransition(new ScaleTransition(button.transform, newScale, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(button.image, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(ButtonStateManager button)
    {
        if (timer <= 0)
        {
            if (!button.uI.activated)
            {
                if (button.uI.Manager.KeyOrControlActive)
                {
                    if (button.uI.Manager.CurrentUISelected == button.uI.position)
                    {
                        button.SwitchState(button.selectedState);
                    }
                    else
                    {
                        button.SwitchState(button.deselectedState);
                    }
                }
                else
                {
                    if (button.uI.Manager.HoveringGameObject(button.gameObject))
                    {
                        button.SwitchState(button.selectedState);
                    }
                    else
                    {
                        button.SwitchState(button.deselectedState);
                    }
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(ButtonStateManager button)
    {
        //button.uI.Manager.Transitioning = true;
        button.uI.actionDelegate?.Invoke();
    }
}
