using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManagerBaseState
{
    public abstract void EnterState(UIStateManager stateManager);
    public abstract void UpdateState(UIStateManager stateManager);
    public abstract void ExitState(UIStateManager stateManager);
}
