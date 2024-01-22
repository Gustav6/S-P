using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchBaseState
{
    public abstract void EnterState(SwitchStateManager @switch);
    public abstract void UpdateState(SwitchStateManager @switch);
    public abstract void ExitState(SwitchStateManager @switch);
}
