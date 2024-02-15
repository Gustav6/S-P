using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#region Button Deselected State
public class ButtonDeselectedState : UIBaseState
{
    private ButtonStateManager manager;
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        if (UIManager.Instance.GetComponentInChildren<PauseManager>() != null)
        {
            if (UIInput.PauseTransitionFinished)
            {
                manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, manager.transform, manager.outlineImage, manager.text);
            }
        }
        else
        {
            manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, manager.transform, manager.outlineImage, manager.text);
        }
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (UIManager.Instance.GetComponentInChildren<PauseManager>() != null)
        {
            if (UIInput.PauseTransitionFinished)
            {
                CheckIfSelected(referenceManager, manager.selectedState);
            }
        }
        else
        {
            CheckIfSelected(referenceManager, manager.selectedState);
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
    private readonly float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        if (!UIManager.Instance.Transitioning)
        {
            manager.StartCoroutine(WaitCoroutine(timeItTakes));
            manager.DefaultSelectTransition(timeItTakes, manager.Pointers, manager.transform, manager.outlineImage, manager.text);
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
#endregion

#region Button Pressed State
public class ButtonPressedState : UIBaseState
{
    private ButtonStateManager manager;
    private Button buttonInstance;

    private readonly float timeItTakes = 0.25f;

    private Vector3 newScale = new(1, 1, 1);
    private Color newOutlineColor = new(1, 1, 1, 1);

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;
        buttonInstance = (Button)referenceManager.UIInstance;

        TransitionSystem.AddScaleTransition(new ScaleTransition(manager.transform, newScale, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(manager.outlineImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));

        referenceManager.StartCoroutine(WaitCoroutine(timeItTakes));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (canTransition && !manager.UIActivated)
        {
            referenceManager.SwitchState(manager.deselectedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        UIManager.Instance.EnableTransitioning();
        buttonInstance.ActivateSelectedFunctions();
    }
}
#endregion

