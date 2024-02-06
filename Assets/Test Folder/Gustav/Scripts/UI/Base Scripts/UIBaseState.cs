using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState
{
    public abstract void EnterState(BaseStateManager referenceManager);
    public abstract void UpdateState(BaseStateManager referenceManager);
    public abstract void ExitState(BaseStateManager referenceManager);

    public bool Hovering(UI UIInstance, UIManager uIManager)
    {
        if (UIManager.ListOfUIObjects.Count > 0 && !UIManager.Transitioning)
        {
            if (UIInstance.GetComponent<Collider2D>().OverlapPoint(uIManager.MousePosition))
            {
                if (UIInstance.GetComponent<UI>() != null)
                {
                    uIManager.currentUISelected = UIInstance.GetComponent<UI>().position;
                    return true;
                }
            }
        }

        return false;
    }
}