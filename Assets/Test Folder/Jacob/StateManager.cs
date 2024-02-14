using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
	internal Dictionary<EState, BaseState<EState>> States = new();

	internal BaseState<EState> CurrentState;

	protected bool IsTransitioningState = false;

	private void Start()
	{
		CurrentState.EnterState();
	}

	private void Update()
	{
		EState nextStateKey = CurrentState.GetNextState();

		if (nextStateKey.Equals(CurrentState.StateKey))
			CurrentState.UpdateState();
		else if (!IsTransitioningState)
			TransitionToState(nextStateKey);
	}

	public void TransitionToState(EState stateKey)
	{
		IsTransitioningState = true;

		CurrentState.ExitState();
		CurrentState = States[stateKey];
		CurrentState.EnterState();

		IsTransitioningState = false;
	}
}
