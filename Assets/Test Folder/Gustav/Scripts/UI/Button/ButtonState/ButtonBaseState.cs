using UnityEngine;

public abstract class ButtonBaseState
{
    public abstract void EnterState(ButtonStateManager button);
    public abstract void UpdateState(ButtonStateManager button);
    public abstract void ExitState(ButtonStateManager button);
}
