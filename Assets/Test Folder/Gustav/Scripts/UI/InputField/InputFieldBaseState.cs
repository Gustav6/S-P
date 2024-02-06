using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputFieldBaseState
{
    public abstract void EnterState(InputFieldStateManager inputField);
    public abstract void UpdateState(InputFieldStateManager inputField);
    public abstract void ExitState(InputFieldStateManager inputField);
}
