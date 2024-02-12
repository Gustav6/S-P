using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WaveCreationState : BaseWaveState
{
	readonly WaveStateContext _context;
	readonly EnemyPreset[] _enemyAssortment;

    public WaveCreationState(WaveStateContext context, WaveStateMachine.WaveState stateKey, EnemyPreset[] enemyAssortment) : base(context, stateKey)
	{
		_context = context;
        _enemyAssortment = enemyAssortment;
	}

	public override void EnterState()
	{
        _context.SetEnemiesToSpawn(GetEnemies(50, WaveStateMachine.WaveType.Balanced));
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

        switch (waveType)
{
            case WaveStateMachine.WaveType.Random:
                for (; ; )
                {
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_context.WaveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    if (remainingBudget < 2)
                        break;

                    finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                    remainingBudget -= randomEnemyCost;
                }
                break;

            case WaveStateMachine.WaveType.Light:
                for (; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

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

                            if (remainingBudget < 2)
                                break;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;

                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;
            case WaveStateMachine.WaveType.Balanced:
                for (; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

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

                            if (remainingBudget < 2)
                                break;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;

                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Heavy:
                for (; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

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

                            if (remainingBudget < 2)
                                break;
                        }
                    }
                    // Spend the rest on other enemies that cost more than 10% of the budget
                    else
                    {
                        if (randomEnemyCost > totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID]);
                            remainingBudget -= randomEnemyCost;


                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;

            case WaveStateMachine.WaveType.Swarm:
                for (; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

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

                            if (remainingBudget < 2)
                                break;
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

        return finalEnemyList;
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
                           AnimationCurve ascensionCurve) : base(context, stateKey)
    {
        _context = context;
        _statRewardInteractables = statRewardInteractables;
        _weaponRewardInteractables = weaponRewardInteractables;
        _statRewardPool = statRewardPool;
        _weaponRewardPool = weaponRewardPool;
        _dropCurve = dropCurve;
        _ascensionCurve = ascensionCurve;
    }

    WaveRewardInteractable[] _statRewardInteractables;
    WaveRewardInteractable[] _weaponRewardInteractables;

    WaveReward[] _statRewardPool;
    WaveReward[] _weaponRewardPool;

    AnimationCurve _dropCurve;
    AnimationCurve _ascensionCurve;

    public override void EnterState()
    {
        GenerateRewards(_statRewardPool, _statRewardInteractables);
        GenerateRewards(_weaponRewardPool, _weaponRewardInteractables);

        EnableStatRewardInteractables();
        EnableWeaponRewardInteractables();
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
            int randomIndex = Random.Range(0, rewardPool.Length);

            while (generatedRewardIndexes.Contains(randomIndex))
                randomIndex = Random.Range(0, rewardPool.Length);

            generatedRewardIndexes.Add(randomIndex);

            interactables[i].SetContainedReward(rewardPool[randomIndex]);

            interactables[i].OnRewardClaimedCallback += (rewardPool[randomIndex] is StatReward) ? DisableStatRewardInteractables : DisableWeaponRewardInteractables;
        }
    }

    void DisableStatRewardInteractables()
    {
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(AscendReward(_statRewardInteractables[i], Random.Range(0, 0.4f)));

        _statRewardsDisabled = true;
    }

    void EnableStatRewardInteractables()
    {
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(DropReward(_statRewardInteractables[i], Random.Range(0, 0.4f)));

        _statRewardsDisabled = false;
    }

    void DisableWeaponRewardInteractables()
    {
        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(AscendReward(_weaponRewardInteractables[i], Random.Range(0, 0.4f)));

        _weaponRewardsDisabled = true;
    }

    void EnableWeaponRewardInteractables()
    {
        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            _context.StateMachine.StartCoroutine(DropReward(_weaponRewardInteractables[i], Random.Range(0, 0.4f)));

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
        rewardInteractable.GetComponent<Animator>().enabled = true;
    }
}


public class WaveIntermissionState : BaseWaveState
{
    WaveStateContext _context;
    Animator _countdownAnim;

    public WaveIntermissionState(WaveStateContext context, WaveStateMachine.WaveState stateKey, Animator countDownAnim) : base(context, stateKey)
	{
		_context = context;
        _countdownAnim = countDownAnim;
	}

    public override void EnterState()
    {
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

    readonly Transform[] _spawnPoints;

    public WaveInProgressState(WaveStateContext context, WaveStateMachine.WaveState stateKey,
                               Image waveProgressFill,
                               RectTransform fishTransform,
                               Animator progressBarAnim,
                               AnimationCurve verticalSpawnCurve,
                               Transform[] spawnPoints) : base(context, stateKey)
	{
		_context = context;
        _waveProgressFill = waveProgressFill;
        _fishTransform = fishTransform;
        _progressBarAnim = progressBarAnim;
        _verticalSpawnCurve = verticalSpawnCurve;
        _spawnPoints = spawnPoints;
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
        _context.StateMachine.TransitionToState(WaveStateMachine.WaveState.WaveCreation);
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

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            SpawnEnemy(enemiesToSpawn[i]);
            _enemyValueAlive += enemiesToSpawn[i].Cost;
        }
    }

    void SpawnEnemy(EnemyPreset enemyPreset)
    {
        Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        GameObject enemy = Object.Instantiate(enemyPreset.EnemyPrefab);
        EnemyLifeStatus enemyLifeStatus = enemy.AddComponent<EnemyLifeStatus>();
        enemyLifeStatus.OnDeathCallback += EnemyDied;
        enemyLifeStatus.Value = enemyPreset.Cost;

        _context.StateMachine.StartCoroutine(AnimateEnemySpawn(enemy.transform, randomSpawnPoint));

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

        float time = 0;

        while (time <= 1.25f)
        {
            yield return null;
            time += Time.deltaTime;
            parentTransform.position = Vector2.Lerp(startPos, endPos, time / 1.25f);
            enemy.localPosition = new Vector2(0, Mathf.Lerp(0, 1.5f, _verticalSpawnCurve.Evaluate(time / 1.25f)));
        }

        enemy.SetParent(null);
		Object.Destroy(parentTransform.gameObject);
    }
}