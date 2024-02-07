using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState
{
    protected bool canTransition;

    public abstract void EnterState(BaseStateManager referenceManager);
    public abstract void UpdateState(BaseStateManager referenceManager);
    public abstract void ExitState(BaseStateManager referenceManager);

    protected void CheckIfSelected(BaseStateManager referenceManager, UIBaseState state)
    {
        if (referenceManager.UIManagerInstance.KeyOrControlActive)
        {
            if (referenceManager.UIManagerInstance.CurrentUISelected == referenceManager.UIInstance.position)
            {
                referenceManager.SwitchState(state);
            }
        }
        else
        {
            if (referenceManager.Hovering(referenceManager.UIInstance, referenceManager.UIManagerInstance))
            {
                referenceManager.SwitchState(state);
            }
        }
    }

    protected void CheckIfDeselected(BaseStateManager referenceManager, UIBaseState state)
    {
        if (referenceManager.UIManagerInstance.KeyOrControlActive)
        {
            if (referenceManager.UIManagerInstance.CurrentUISelected != referenceManager.UIInstance.position)
            {
                referenceManager.SwitchState(state);
            }
        }
        else
        {
            if (!referenceManager.Hovering(referenceManager.UIInstance, referenceManager.UIManagerInstance))
            {
                referenceManager.SwitchState(state);
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