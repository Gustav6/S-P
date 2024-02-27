using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;
using System;

public class WaveCreationState : BaseWaveState
{
	readonly WaveStateContext _context;
	readonly EnemyPreset[] _enemyAssortment;

    WaveInfoPopup _waveInfoPopup;

    public WaveCreationState(WaveStateContext context, WaveStateMachine.WaveState stateKey, EnemyPreset[] enemyAssortment, WaveInfoPopup waveInfoPopup) : base(context, stateKey)
	{
		_context = context;
        _enemyAssortment = enemyAssortment;
        _waveInfoPopup = waveInfoPopup;
	}

	public override void EnterState()
	{
        _context.SetEnemiesToSpawn(GetEnemies((int)(_context.WaveNumber / 2f * 50f + (_context.WaveNumber * 5f)), (WaveStateMachine.WaveType)UnityEngine.Random.Range(0, 5)));
        _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.Reward);
	}

	public override void ExitState()
	{

	}

	public override WaveStateMachine.WaveState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{

    }

    List<EnemyPreset> GetEnemies(int totalBudget, WaveStateMachine.WaveType waveType)
    {
        List<EnemyPreset> finalEnemyList = new();

        int remainingBudget = totalBudget;

        // Generating enemy list
        switch (waveType)
{
            case WaveStateMachine.WaveType.Random:
                for (;;)
                {
                    if (remainingBudget < 3)
                        break;

                    int randomEnemyID = UnityEngine.Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                    remainingBudget -= randomEnemyCost;
                }
                break;

            case WaveStateMachine.WaveType.Light:
                for (;;)
                {
                    if (remainingBudget < 3)
                        break;

                    // Get random enemy
                    int randomEnemyID = UnityEngine.Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 60% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.40f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth less than 10% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth more than 10% of the budget, we pick enemies at random
						{
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Balanced:
                for (;;)
                {
                    if (remainingBudget < 3)
                        break;

                    // Get random enemy
                    int randomEnemyID = UnityEngine.Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 40% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.60f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth less than 10% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth more than 10% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Heavy:
                for (;;)
                {
                    if (remainingBudget < 3)
                        break;

                    // Get random enemy
                    int randomEnemyID = UnityEngine.Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 10% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.90f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment less more than 10% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth more than 10% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Swarm:
                for (;;)
                {
                    if (remainingBudget < 3)
                        break;

                    // Get random enemy
                    int randomEnemyID = UnityEngine.Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 100% of budget on enemies with a cost lower than 15% of the budget
                    if (remainingBudget >= 0)
                    {
                        if (randomEnemyCost <= totalBudget * 0.15f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                        else // If there are no enemies in the assortment worth less than 15% of the budget, we pick enemies at random
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Boss:
                EnemyPreset wealthiestEnemy = _enemyAssortment[0];

                foreach (EnemyPreset e in _enemyAssortment)
                {
                    if (e.Cost > wealthiestEnemy.Cost && _context.WaveNumber >= e.WaveUnlocked)
                        wealthiestEnemy = e;
                }

                finalEnemyList.Add(wealthiestEnemy);
                break;
        }

        // Displaying wave info
        switch (waveType)
        {
            case WaveStateMachine.WaveType.Random:
                _waveInfoPopup.SetText("Random Wave", "All foes are chosen at random, expect the unexpected.");
                break;

            case WaveStateMachine.WaveType.Balanced:
                _waveInfoPopup.SetText("Balanced Wave", "An equal balance between weak and strong foes.");
                break;

            case WaveStateMachine.WaveType.Light:
                _waveInfoPopup.SetText("Light Wave", "The enemies strength lies in their numbers, expect a large amount of weaker foes.");
                break;

            case WaveStateMachine.WaveType.Heavy:
                _waveInfoPopup.SetText("Heavy Wave", "Prepare to brawl with an array of larger foes, don't get careless.");
                break;

            case WaveStateMachine.WaveType.Swarm:
                _waveInfoPopup.SetText("Swarm Wave", "Massive hordes of weaker enemies incoming, expect to be overwhelmed.");
                break;

            case WaveStateMachine.WaveType.Boss:
                _waveInfoPopup.SetText("Boss Wave", "ooga booga big boss incoming!!");
                break;

            default:
                _waveInfoPopup.SetText("Fuck you wave", "This wave is not supposed to exist, get out of here.");
                break;
        }

        _waveInfoPopup.Enable();

        System.Random random = new System.Random();
        return finalEnemyList.ToArray().OrderBy(x => random.Next()).ToList();
    }
}


public class WaveRewardState : BaseWaveState
{
    readonly WaveStateContext _context;

    bool _weaponRewardsDisabled, _statRewardsDisabled;

    public WaveRewardState(WaveStateContext context, WaveStateMachine.WaveState stateKey, 
                           WaveRewardInteractable[] statRewardInteractables, 
                           WaveRewardInteractable[] weaponRewardInteractables,
                           WaveReward[] statRewardPool,
                           WaveReward[] weaponRewardPool,
                           AnimationCurve dropCurve,
                           AnimationCurve ascensionCurve,
                           int statRewardIncrement,
                           int weaponRewardIncrement,
                           TutorialManager tutorialManager) : base(context, stateKey)
    {
        _context = context;
        _statRewardInteractables = statRewardInteractables;
        _weaponRewardInteractables = weaponRewardInteractables;
        _statRewardPool = statRewardPool;
        _weaponRewardPool = weaponRewardPool;
        _dropCurve = dropCurve;
        _ascensionCurve = ascensionCurve;
        _statRewardIncrement = statRewardIncrement;
        _weaponRewardIncrement = weaponRewardIncrement;
        _tutorialManager = tutorialManager;
    }

    WaveRewardInteractable[] _statRewardInteractables;
    WaveRewardInteractable[] _weaponRewardInteractables;

    WaveReward[] _statRewardPool;
    WaveReward[] _weaponRewardPool;

    AnimationCurve _dropCurve;
    AnimationCurve _ascensionCurve;

    TutorialManager _tutorialManager;

    int _statRewardIncrement, _weaponRewardIncrement;

    public override void EnterState()
    {
        if (((_context.WaveNumber - 1) % _statRewardIncrement) == 0 || (_context.WaveNumber - 1) == 0)
        {
            GenerateRewards(_statRewardPool, _statRewardInteractables);
            EnableStatRewardInteractables();
        }

        if (((_context.WaveNumber - 1) % _weaponRewardIncrement) == 0 || (_context.WaveNumber - 1) == 0)
        {
            GenerateRewards(_weaponRewardPool, _weaponRewardInteractables);
            EnableWeaponRewardInteractables();
        }
    }

	public override void ExitState()
	{
        // Safeguard in case we somehow exit reward state without claiming a reward.
        if (!_weaponRewardsDisabled)
            DisableWeaponRewardInteractables();

        if (!_statRewardsDisabled)
            DisableStatRewardInteractables();
	}

	public override WaveStateMachine.WaveState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
        if (_statRewardsDisabled && _weaponRewardsDisabled)
            _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.Intermission);
	}

    void GenerateRewards(WaveReward[] rewardPool, WaveRewardInteractable[] interactables)
    {
        List<int> generatedRewardIndexes = new();

        for (int i = 0; i < interactables.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, rewardPool.Length);

            while (generatedRewardIndexes.Contains(randomIndex) || _context.WaveNumber < rewardPool[randomIndex].WaveUnlocked)
                randomIndex = UnityEngine.Random.Range(0, rewardPool.Length);

            generatedRewardIndexes.Add(randomIndex);

            interactables[i].SetContainedReward(rewardPool[randomIndex]);

            interactables[i].OnRewardClaimedCallback += (rewardPool[randomIndex] is StatReward) ? DisableStatRewardInteractables : DisableWeaponRewardInteractables;
        }
    }

    void DisableStatRewardInteractables()
    {
        _tutorialManager.OnStatPickup();
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(AscendReward(_statRewardInteractables[i], UnityEngine.Random.Range(0, 0.4f)));

        _statRewardsDisabled = true;
    }

    void EnableStatRewardInteractables()
    {
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(DropReward(_statRewardInteractables[i], UnityEngine.Random.Range(0, 0.4f)));

        _statRewardsDisabled = false;
    }

    void DisableWeaponRewardInteractables()
    {
        _tutorialManager.OnWeaponPickup();
        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(AscendReward(_weaponRewardInteractables[i], UnityEngine.Random.Range(0, 0.4f)));

        _weaponRewardsDisabled = true;
    }

    void EnableWeaponRewardInteractables()
    {
        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(DropReward(_weaponRewardInteractables[i], UnityEngine.Random.Range(0, 0.4f)));

        _weaponRewardsDisabled = false;
    }

    IEnumerator AscendReward(WaveRewardInteractable rewardInteractable, float initialDelay)
    {
        rewardInteractable.GetComponent<Animator>().enabled = false;
        rewardInteractable.enabled = false;

        yield return new WaitForSeconds(initialDelay);

        Transform spriteTransform = rewardInteractable.transform.GetChild(0);

        Vector2 endPos = rewardInteractable.transform.position + new Vector3(0, 11.5f);

        float time = 0;

        while (time <= 0.75f)
        {
            time += Time.deltaTime;
            yield return null;
            spriteTransform.position = new Vector2(rewardInteractable.transform.position.x, Mathf.Lerp(rewardInteractable.transform.position.y, endPos.y, _ascensionCurve.Evaluate(time / 0.75f)));
        }

        spriteTransform.position = new Vector2(rewardInteractable.transform.position.x, endPos.y);
        rewardInteractable.GetComponent<Animator>().Play("Clear");
    }

    IEnumerator DropReward(WaveRewardInteractable rewardInteractable, float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        Transform spriteTransform = rewardInteractable.transform.GetChild(0);

        Vector2 startPos = rewardInteractable.transform.position + new Vector3(0, 11.5f);

        float time = 0;

        while (time <= 0.75f)
        {
            time += Time.deltaTime;
            yield return null;
            spriteTransform.transform.position = new Vector2(startPos.x, Mathf.Lerp(startPos.y, rewardInteractable.transform.position.y, _dropCurve.Evaluate(time / 0.75f)));
        }

        spriteTransform.position = new Vector2(startPos.x, rewardInteractable.transform.position.y);

        rewardInteractable.enabled = true;
        rewardInteractable.GetComponent<Animator>().Play("WaveRewardBob");
    }
}


public class WaveIntermissionState : BaseWaveState
{
    WaveStateContext _context;
    Animator _countdownAnim;
    WaveInfoPopup _waveInfoPopup;

    public WaveIntermissionState(WaveStateContext context, WaveStateMachine.WaveState stateKey, Animator countDownAnim, WaveInfoPopup waveInfoPopup) : base(context, stateKey)
	{
		_context = context;
        _countdownAnim = countDownAnim;
        _waveInfoPopup = waveInfoPopup;
	}

    public override void EnterState()
    {
        _waveInfoPopup.Disable();
        _countdownAnim.Play("Countdown");
    }

    public override void ExitState()
    {

    }

    public override WaveStateMachine.WaveState GetNextState()
    {
        return StateKey;
    }

    public override void UpdateState()
    {
        if (_countdownAnim.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.WaveInProgress);
        }
    }
}


public class WaveInProgressState : BaseWaveState
{
	readonly WaveStateContext _context;

    float _desiredFishPos = -824;
    float _desiredFillAmount = 0;

    int _enemyCountThisWave;
    int _enemiesKilled = 0;
    int _enemyValueAlive;

	readonly Image _waveProgressFill;
    readonly RectTransform _fishTransform;
    readonly Animator _progressBarAnim;

    readonly AnimationCurve _verticalSpawnCurve;

    readonly Transform _waterPoolPrefab;

    public WaveInProgressState(WaveStateContext context, WaveStateMachine.WaveState stateKey,
                               Image waveProgressFill,
                               RectTransform fishTransform,
                               Animator progressBarAnim,
                               AnimationCurve verticalSpawnCurve,
                               Transform waterPoolPrefab) : base(context, stateKey)
	{
		_context = context;
        _waveProgressFill = waveProgressFill;
        _fishTransform = fishTransform;
        _progressBarAnim = progressBarAnim;
        _verticalSpawnCurve = verticalSpawnCurve;
        _waterPoolPrefab = waterPoolPrefab;
	}

	public override void EnterState()
	{
        _progressBarAnim.Play("ProgressBarActivate");
        _context.StateMachine.StartCoroutine(SpawnEnemies(_context.EnemiesToSpawn));
        _enemyCountThisWave = _context.EnemiesToSpawn.Length;
        SetWaveProgress(0);
	}

	public override void ExitState()
	{
        _progressBarAnim.Play("ProgressBarDeActivate");
        _context.NextWave();
    }

	public override WaveStateMachine.WaveState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
        _fishTransform.anchoredPosition = new Vector2(Mathf.Lerp(_fishTransform.anchoredPosition.x, _desiredFishPos, Time.deltaTime * 3), _fishTransform.anchoredPosition.y);
        _waveProgressFill.fillAmount = Mathf.Lerp(_waveProgressFill.fillAmount, _desiredFillAmount, Time.deltaTime * 3);

        if (_enemiesKilled >= _enemyCountThisWave && _waveProgressFill.fillAmount >= 0.999f)
            WaveCleared();
    }

    void WaveCleared()
	{
        _enemiesKilled = 0;
        SetWaveProgress(0);
        _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.WaveCleared);
    }

    void SetWaveProgress(float progress)
    {
        _desiredFishPos = (progress * (446 + 824)) - 824;
        _desiredFillAmount = progress;
    }

    IEnumerator SpawnEnemies(EnemyPreset[] enemiesToSpawn)
    {
        int maxValueAlive = (int)(_context.WaveNumber * 1.25f + _context.WaveNumber / 2f * 10f);

        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (_enemyValueAlive >= maxValueAlive)
            {
                i--;
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
            if (!_context.CanSpawnEnemies)
                yield break;

            SpawnEnemy(enemiesToSpawn[i]);
            _enemyValueAlive += enemiesToSpawn[i].Cost;
        }
    }

    void SpawnEnemy(EnemyPreset enemyPreset)
    {
        Transform randomSpawnPoint = _context.SpawnPointParent.GetChild(UnityEngine.Random.Range(0, _context.SpawnPointParent.childCount));

        GameObject enemy = UnityEngine.Object.Instantiate(enemyPreset.EnemyPrefab);
        EnemyLifeStatus enemyLifeStatus = enemy.AddComponent<EnemyLifeStatus>();
        enemyLifeStatus.OnDeathCallback += EnemyDied;
        enemyLifeStatus.Value = enemyPreset.Cost;
        enemy.SetActive(false);

        _context.StateMachine.StartCoroutine(AnimateEnemySpawn(enemy.transform, randomSpawnPoint));

        _context.SpawnedEnemies.Add(enemy.GetComponent<Enemy>());

        // Enable enemy AI.
    }

    void EnemyDied(int value)
	{
        _enemyValueAlive -= value;
        _enemiesKilled++;
        SetWaveProgress((float)_enemiesKilled / _enemyCountThisWave);
    }

    IEnumerator AnimateEnemySpawn(Transform enemy, Transform spawnPoint)
    {
        Transform parentTransform = new GameObject().transform;
        enemy.position = parentTransform.position;
        enemy.SetParent(parentTransform);

        Vector2 startPos = spawnPoint.GetChild(0).position;
        Vector2 endPos = spawnPoint.GetChild(1).position;

        parentTransform.position = startPos;

        Transform waterPool = UnityEngine.Object.Instantiate(_waterPoolPrefab, startPos, Quaternion.identity);
        waterPool.localScale = new Vector2(waterPool.localScale.x * Mathf.Sign(endPos.x - waterPool.transform.position.x), waterPool.localScale.y);

        // Waits for a bit while it's just the whirlpool
        yield return new WaitForSeconds(0.9f);

        float time = 0;

        while (time <= 1f)
        {
            yield return null;
            time += Time.deltaTime;
            parentTransform.position = Vector2.Lerp(startPos, endPos, time);
            enemy.localPosition = new Vector2(0, Mathf.Lerp(0, 1.5f, _verticalSpawnCurve.Evaluate(time)));
            waterPool.position = enemy.position;
        }

        enemy.gameObject.SetActive(true);
        enemy.SetParent(null);
        UnityEngine.Object.Destroy(parentTransform.gameObject, 0.25f);
        UnityEngine.Object.Destroy(waterPool.gameObject, 0.25f);
    }
}


public class WaveClearState : BaseWaveState
{
    WaveStateContext _context;
    string[] _messages;
    TMP_Text _messageText;

    Animator _animator;

    public WaveClearState(WaveStateContext context, WaveStateMachine.WaveState stateKey, string[] messages, TMP_Text messageText, Animator animator) : base(context, stateKey)
    {
        _context = context;
        _messages = messages;
        _messageText = messageText;
        _animator = animator;
    }

    public override void EnterState()
    {
        _messageText.text = _messages[UnityEngine.Random.Range(0, _messages.Length)];
        _animator.Play("WaveClear");
        PlayerStats.Instance.ResetDamage();
        PlayerStats.Instance.ClearEquippedAbility();
        EquipmentManager.Instance.DestroySpawnedPowerUp();

    }

    public override void ExitState()
    {

    }

    public override WaveStateMachine.WaveState GetNextState()
    {
        return StateKey;
    }

    public override void UpdateState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.StageSwap);
        }
    }
}


public class StageSwapState : BaseWaveState
{
    WaveStateContext _context;

    Animator _armAnim;

    float _timer;
    bool _isSwapping;
    bool _cancel;

    IslandTransition[] _islandPrefabs;
    IslandTransition _previousIsland;

    Transform _gridTransform;

    WaveRewardInteractable[] _statRewardInteractables, _weaponRewardInteractables;

    Transform _rewardParent;

    Vector2 _startPos;
    Transform _armCenter;

    int _stageSwapIncrement;


    public StageSwapState(WaveStateContext context, WaveStateMachine.WaveState stateKey, 
                          Animator armAnim, 
                          IslandTransition[] islandPrefabs, 
                          IslandTransition currentIsland, 
                          Transform gridTransform, 
                          WaveRewardInteractable[] statRewardInteractables, 
                          WaveRewardInteractable[] weaponRewardInteractables, 
                          int stageSwapIncrement) : base(context, stateKey)
    {
        _context = context;
        _armAnim = armAnim;
        _islandPrefabs = islandPrefabs;
        _previousIsland = currentIsland;
        _gridTransform = gridTransform;
        _statRewardInteractables = statRewardInteractables;
        _weaponRewardInteractables = weaponRewardInteractables;
        _stageSwapIncrement = stageSwapIncrement;

        _armCenter = armAnim.transform.GetChild(0);
    }

    public override void EnterState()
    {
        if ((_context.WaveNumber % _stageSwapIncrement) != 0)
		{
            _cancel = true;
            _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.WaveCreation);
            return;
        }

        _cancel = false;
        _timer = 0;
        _isSwapping = false;
        TransitionManager.Instance.ApproachPlayer();
        _startPos = _armCenter.position;
        PlayerStats.Instance.ToggleInvulnerability(true);
    }

    public override void ExitState()
    {
        PlayerStats.Instance.ToggleInvulnerability(false);
        AstarPath.active.Scan();
    }

    public override WaveStateMachine.WaveState GetNextState()
    {
        return StateKey;
    }

    // Property of Tristan
    public void GenerateIsland()
    {
        IslandTransition randomIsland = _islandPrefabs[UnityEngine.Random.Range(0, _islandPrefabs.Length)];

        IslandTransition island = UnityEngine.Object.Instantiate(randomIsland, new Vector3(-18, 0, 0), Quaternion.identity, _gridTransform);
        island.transform.position = new Vector3(-18, 0, 0);

        island.SwapIsland();
        _previousIsland.SwapIsland();
        _previousIsland = island;

        _rewardParent = island.transform.GetChild(0);

        _context.SetSpawnPointsParent(island.transform.GetChild(1));
        _context.SetRespawnPoint(island.transform.GetChild(2));
        EquipmentManager.Instance.SetPowerUpSpawnPoints(_context.SpawnPointParent);

        PlayerStats.Instance.UpdateTilemap(island.GetComponent<Tilemap>());
    }

    public override void UpdateState()
    {
        if (_cancel)
            return;

        if (!_isSwapping && _armAnim.GetCurrentAnimatorStateInfo(0).IsName("ArmPickUp"))
        {
            if (_timer == 0)
                GenerateIsland();

            _timer += Time.deltaTime;

            if (_timer >= 1.5f)
            {
                for (int i = 0; i < 2; i++)
                    _weaponRewardInteractables[i].transform.position = _rewardParent.GetChild(i).position;

                for (int i = 2; i < 5; i++)
                    _statRewardInteractables[i - 2].transform.position = _rewardParent.GetChild(i).position;

                _timer = 0;
                _isSwapping = true;
                TransitionManager.Instance.DropPlayer();
            }
        }
        else if (_isSwapping)
        {
            _timer += Time.deltaTime;

            _armCenter.position = Vector2.Lerp(_startPos, _context.RespawnPoint.position, _timer / 0.3f);

            if (_timer >= 0.3f)
            {
                _isSwapping = false;
            }
        }

        if (_armAnim.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.WaveCreation);
        }
    }
}


public class WaveLossState : BaseWaveState
{
    WaveStateContext _context;

    private GameObject _gameOverObject;

    public WaveLossState(WaveStateContext context, WaveStateMachine.WaveState stateKey, GameObject gameOver) : base(context, stateKey)
    {
        _context = context;
        _gameOverObject = gameOver;
    }

    public override void EnterState()
    {
        _gameOverObject.SetActive(true);

        _context.CanSpawnEnemies = false;

        foreach(Enemy enemy in _context.SpawnedEnemies)
        {
            if (enemy == null)
                continue;

            enemy._enemyAttack.CanAttack(false);

            enemy._attackController.LeaveAttack();
            enemy._attackController.LeaveMovement(true);
        }
    }

    public override void ExitState()
    {
        _gameOverObject.SetActive(false);
        // TODO: Play animation to remove Gameover Prefab.
    }

    public override WaveStateMachine.WaveState GetNextState()
    {
        return StateKey;
    }

    public override void UpdateState()
    {

    }
}