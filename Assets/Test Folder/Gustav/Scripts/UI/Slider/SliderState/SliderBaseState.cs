using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderBaseState
{
    public abstract void EnterState(SliderStateManager slider);
    public abstract void UpdateState(SliderStateManager slider);
    public abstract void ExitState(SliderStateManager slider);
}