using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState
{
    protected bool canTransition;

    public abstract void EnterState(BaseStateManager referenceManager);
    public abstract void UpdateState(BaseStateManager referenceManager);
    public abstract void ExitState(BaseStateManager referenceManager);

    protected void CheckIfSelected(BaseStateManager referenceManager, UIBaseState selectedState)
    {
        if (UIStateManager.Instance.KeyOrControlActive)
        {
            if (UIStateManager.Instance.CurrentUISelected == referenceManager.UIInstance.position)
            {
                referenceManager.SwitchState(selectedState);
            }
        }
        else
        {
            if (referenceManager.UIInstance.hovering)
            {
                referenceManager.SwitchState(selectedState);
            }
        }
    }

    protected void CheckIfDeselected(BaseStateManager referenceManager, UIBaseState deselectedState)
    {
        if (UIStateManager.Instance != null)
        {
            if (UIStateManager.Instance.Transitioning)
            {
                referenceManager.SwitchState(deselectedState);
            }
        }

        if (UIStateManager.Instance.KeyOrControlActive)
        {
            if (UIStateManager.Instance.CurrentUISelected != referenceManager.UIInstance.position)
            {
                referenceManager.SwitchState(deselectedState);
            }
        }
        else
        {
            if (!referenceManager.UIInstance.hovering)
            {
                referenceManager.SwitchState(deselectedState);
            }
        }
    }

    protected bool HasPressed(BaseStateManager referenceManager)
    {
        if (referenceManager.UIActivated)
        {
            return true;
        }

        return false;
    }

    protected IEnumerator WaitCoroutine(float time)
    {
        canTransition = false;

        yield return new WaitForSeconds(time);

        canTransition = true;
    }
}