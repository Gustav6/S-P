using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#region Deselected State
public class ButtonDeselectedState : UIBaseState
{
    private ButtonStateManager manager;
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, manager.outline, manager.backgroundImage, manager.transform, manager.text);
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(referenceManager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Hover");
        }
    }
}
#endregion

#region Selected State
public class ButtonSelectedState : UIBaseState
{
    private ButtonStateManager manager;
    private readonly float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (ButtonStateManager)referenceManager;

        if (!UIStateManager.Instance.Transitioning)
        {
            manager.StartCoroutine(WaitCoroutine(timeItTakes));
            manager.DefaultSelectTransition(timeItTakes, manager.Pointers, manager.outline, manager.backgroundImage, manager.transform, manager.text);
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

#region Pressed State
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Click");
        }

        TransitionSystem.AddScaleTransition(new ScaleTransition(manager.transform, newScale, timeItTakes, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(manager.backgroundImage, newOutlineColor, timeItTakes, TransitionType.SmoothStart2));

        referenceManager.StartCoroutine(WaitCoroutine(timeItTakes));
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        if (canTransition && !manager.UIActivated)
        {
            referenceManager.SwitchState(manager.selectedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        buttonInstance.ActivateSelectedFunctions();
    }
}
#endregion

