using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WaveStateMachine : StateManager<WaveStateMachine.WaveState>
{
    public enum WaveState
	{ 
		WaveCreation,
		Reward,
		Intermission,
		WaveInProgress
	}

	public enum WaveType
	{
		Random,
		Light,
		Balanced,
		Heavy,
		Swarm,
		Boss
	}

	WaveStateContext _context;

	[SerializeField] EnemyPreset[] _enemyAssortment;

	[SerializeField] WaveRewardInteractable[] _statRewardInteractables, _weaponRewardInteractables;
	[SerializeField] WaveReward[] _statRewardPool, _weaponRewardPool;

	[SerializeField] AnimationCurve _dropCurve, _ascensionCurve, _verticalSpawnCurve;

	[SerializeField] Transform[] _spawnPoints;

	[SerializeField] Image _waveProgressFill;
	[SerializeField] RectTransform _fishTransform;
	[SerializeField] Animator _progressBarAnim;

	private void Awake()
	{
		ValidateValues();

		_context = new(this);

		InitializeStates();
	}

	public void ValidateValues()
	{
		Assert.IsNotNull(_enemyAssortment, "Enemy Assortment is not assigned");
	}

	void InitializeStates()
	{
		States.Add(WaveState.WaveCreation, new WaveCreationState(_context, WaveState.WaveCreation, _enemyAssortment));
		States.Add(WaveState.Reward, new WaveRewardState(_context, WaveState.Reward, _statRewardInteractables, _weaponRewardInteractables, _statRewardPool, _weaponRewardPool, _dropCurve, _ascensionCurve));
		States.Add(WaveState.Intermission, new WaveIntermissionState(_context, WaveState.Intermission));
		States.Add(WaveState.WaveInProgress, new WaveInProgressState(_context, WaveState.WaveInProgress, _waveProgressFill, _fishTransform, _progressBarAnim, _verticalSpawnCurve, _spawnPoints));
		CurrentState = States[WaveState.WaveCreation];
	}
}
