using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtenDeselectedState : ButtonBaseState
{
    private float timeItTakes = 0.25f;

    private Color newOutlineColor = new(0, 0, 0, 0.5f);
    private Color newTextColor = new(1, 1, 1, 0.5f);

    public override void EnterState(ButtonStateManager button)
    {
        button.pointers.gameObject.SetActive(false);

        TransitionSystem.AddColorTransition(new ColorTransition(button.image, newOutlineColor, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(button.text, newTextColor, timeItTakes, TransitionType.SmoothStop2));

        if (!UIManager.Transitioning)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(button.transform, Vector3.one, timeItTakes, TransitionType.SmoothStart2));
        }
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
