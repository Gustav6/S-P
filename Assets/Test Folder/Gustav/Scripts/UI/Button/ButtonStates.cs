using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Button Deselected State
public class ButtonDeselectedState : UIBaseState
{
    private ButtonStateManager manager;
    private float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.pointers, manager.transform, manager.outlineImage, manager.text);
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
            if (Hovering(manager.UIInstance, manager.UIManagerInstance))
            {
                manager.SwitchState(manager.selectedState);
            }
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        manager.AudioManagerInstance.PlaySound(AudioType.SelectSound);
    }
}
#endregion

#region Button Selected State
public class ButtonSelectedState : UIBaseState
{
    private ButtonStateManager manager;
    private float timer;
    private float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager stateManager)
    {
        manager = (ButtonStateManager)stateManager;

        if (!UIManager.Transitioning)
        {
            stateManager.DefaultSelectTransition(timeItTakes, manager.pointers, manager.transform, manager.outlineImage, manager.text);
        }
        else
        {
            stateManager.SwitchState(manager.deselectedState);
        }
    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        if (stateManager.UIManagerInstance.KeyOrControlActive)
        {
            if (stateManager.UIManagerInstance.CurrentUISelected != manager.UIInstance.position)
            {
                stateManager.SwitchState(manager.deselectedState);
            }
        }
        else
        {
            if (!Hovering(stateManager.UIInstance, stateManager.UIManagerInstance))
            {
                stateManager.SwitchState(manager.deselectedState);
            }
        }

        if (timer <= 0)
        {
            if (stateManager.UIActivated)
            {
                stateManager.SwitchState(manager.pressedState);
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
#endregion

#region Button Pressed State
public class ButtonPressedState : UIBaseState
{
    private ButtonStateManager manager;

    private float timer;
    private float timeItTakes = 0.25f;

    private Vector3 newScale = new(1, 1, 1);
    private Color newOutlineColor = new(1, 1, 1, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        timer = timeItTakes;
        TransitionSystem.AddScaleTransition(new ScaleTransition(manager.transform, newScale, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(manager.outlineImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (timer <= 0)
        {
            if (!manager.UIActivated)
            {
                if (manager.UIManagerInstance.KeyOrControlActive)
                {
                    if (manager.UIManagerInstance.CurrentUISelected == manager.UIInstance.position)
                    {
                        manager.SwitchState(manager.selectedState);
                    }
                    else
                    {
                        manager.SwitchState(manager.deselectedState);
                    }
                }
                else
                {
                    if (manager.UIManagerInstance.HoveringGameObject(manager.gameObject))
                    {
                        manager.SwitchState(manager.selectedState);
                    }
                    else
                    {
                        manager.SwitchState(manager.deselectedState);
                    }
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        Button button = (Button)referenceManager.UIInstance;

        Transition.ExecuteOnCompletion @delegate = null;

        if (button.transitionToPrefab)
        {
            UIManager.EnableTransitioning();

            if (button.prefabMoveTransition)
            {
                //button.MovePrefabToDestination(button.methods.InstantiatePrefab, 1);
                Debug.Log("Move In Prefab");
            }
            else if (button.prefabScaleTransition)
            {
                //button.methods.ShrinkTransition(button.methods.InstantiatePrefab, 1);
                Debug.Log("Scale In Prefab");
            }
        }

        if (button.transitionToScene)
        {
            UIManager.EnableTransitioning();

            //@delegate += button.methods.SwitchScene;
            //button.methods.ShrinkTransition(null, 1);
            //PanelManager.FadeOut(0.8f, new Color(0, 0, 0, 1), @delegate);
            Debug.Log("Change Scene");
        }
    }
}
#endregion

