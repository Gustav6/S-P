using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

public class WaveStateMachine : StateManager<WaveStateMachine.WaveState>
{
	public enum WaveState
	{
		WaveCreation,
		StageSwap,
		Reward,
		Intermission,
		WaveInProgress,
		WaveCleared,
		WaveLoss
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

	[SerializeField] Transform _spawnPointsParent;

	[SerializeField] Image _waveProgressFill;
	[SerializeField] RectTransform _fishTransform;
	[SerializeField] Animator _progressBarAnim, _countDownAnim, _armAnim;

	[SerializeField] string[] _clearMessages;
	[SerializeField] TMP_Text _messageText;
	[SerializeField] Animator _waveClearAnim;

	[SerializeField] IslandGenerator _islandGenerator;
	[SerializeField] IslandTransition[] _islandPrefabs;
	[SerializeField] IslandTransition _currentIsland;
	[SerializeField] Transform _gridTransform;

	[SerializeField] Transform _respawnPoint;

	[SerializeField] WaveInfoPopup _waveInfoPopup;

	[SerializeField] GameObject _gameOverUIObject;

	private void Awake()
	{
		_context = new(this, _spawnPointsParent, _respawnPoint);

		InitializeStates();
	}

	void InitializeStates()
	{
		States.Add(WaveState.WaveCreation, new WaveCreationState(_context, WaveState.WaveCreation, _enemyAssortment, _waveInfoPopup));
		States.Add(WaveState.Reward, new WaveRewardState(_context, WaveState.Reward, _statRewardInteractables, _weaponRewardInteractables, _statRewardPool, _weaponRewardPool, _dropCurve, _ascensionCurve));
		States.Add(WaveState.Intermission, new WaveIntermissionState(_context, WaveState.Intermission, _countDownAnim, _waveInfoPopup));
		States.Add(WaveState.WaveInProgress, new WaveInProgressState(_context, WaveState.WaveInProgress, _waveProgressFill, _fishTransform, _progressBarAnim, _verticalSpawnCurve));
		States.Add(WaveState.WaveCleared, new WaveClearState(_context, WaveState.WaveCleared, _clearMessages, _messageText, _waveClearAnim));
		States.Add(WaveState.StageSwap, new StageSwapState(_context, WaveState.StageSwap, _armAnim, _islandPrefabs, _currentIsland, _gridTransform, _statRewardInteractables, _weaponRewardInteractables));
		States.Add(WaveState.WaveLoss, new WaveLossState(_context, WaveState.WaveLoss, _gameOverUIObject));

		CurrentState = States[WaveState.WaveCreation];
	}
}