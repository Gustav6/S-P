using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWaveState : BaseState<WaveStateMachine.WaveState>
{
    protected WaveStateContext Context;

	public BaseWaveState(WaveStateContext context, WaveStateMachine.WaveState stateKey) : base(stateKey)
	{
		Context = context;
	}
}
