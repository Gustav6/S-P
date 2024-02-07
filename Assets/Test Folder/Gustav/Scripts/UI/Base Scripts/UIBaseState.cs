using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState
{
    public abstract void EnterState(BaseStateManager referenceManager);
    public abstract void UpdateState(BaseStateManager referenceManager);
    public abstract void ExitState(BaseStateManager referenceManager);
}