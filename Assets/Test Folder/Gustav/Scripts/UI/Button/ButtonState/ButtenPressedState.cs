using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtenPressedState : ButtonBaseState
{
    private float timer;
    private float timeItTakes = 0.15f;

    private Vector3 newScale = new(1, 1, 1);
    private Color fadeOutColor = new(0, 0, 0, 1);
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
                if (button.uI.UIManagerInstance.KeyOrControlActive)
                {
                    if (button.uI.UIManagerInstance.CurrentUISelected == button.uI.position)
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
                    if (button.uI.UIManagerInstance.HoveringGameObject(button.gameObject))
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
        Transition.ExecuteOnCompletion @delegate = null;

        if (button.methods.TransitionToPrefab)
        {
            UIManager.EnableTransitioning();

            if (button.methods.PrefabMoveTransition)
            {
                button.methods.MovePrefabToDestination(button.methods.InstantiatePrefab, 1);
            }
            else if (button.methods.PrefabScaleTransition)
            {
                button.methods.ShrinkTransition(button.methods.InstantiatePrefab, 1);
            }
        }

        if (button.methods.TransitionToScene)
        {
            UIManager.EnableTransitioning();

            @delegate += button.methods.SwitchScene;
            button.methods.ShrinkTransition(null, 1);
            PanelManager.FadeOut(0.8f, new Color(0, 0, 0, 1), @delegate);
        }
    }
}
