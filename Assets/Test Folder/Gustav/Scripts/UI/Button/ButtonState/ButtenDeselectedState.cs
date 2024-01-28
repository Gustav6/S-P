using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtenDeselectedState : ButtonBaseState
{
    private Vector3 newScale = new(0.925f, 0.925f, 1);
    private float timeItTakes = 0.25f;

    private Color newOutlineColor = new(0, 0, 0, 0.5f);
    private Color newTextColor = new(1, 1, 1, 0.5f);

    public override void EnterState(ButtonStateManager button)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(button.image, newOutlineColor, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(button.text, newTextColor, timeItTakes, TransitionType.SmoothStop2));

        if (button.uI.Manager != null && !button.uI.Manager.Transitioning)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(button.transform, newScale, timeItTakes, TransitionType.SmoothStart2));
        }
    }

    public override void UpdateState(ButtonStateManager button)
    {
        if (button.uI.Manager.KeyOrControlActive)
        {
            if (button.uI.Manager.CurrentUISelected == button.uI.position)
            {
                button.SwitchState(button.selectedState);
            }
        }
        else
        {
            if (button.uI.Manager.HoveringGameObject(button.gameObject))
            {
                button.SwitchState(button.selectedState);
            }
        }
    }
    public override void ExitState(ButtonStateManager button)
    {

    }
}
