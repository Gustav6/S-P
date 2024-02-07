using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] EnemyPreset[] _enemyAssortment;
    [SerializeField] List<Transform> _spawnPoints;
    public GameObject StatRewardPanel, WeaponRewardPanel;

    [SerializeField] WaveRewardInteractable[] _statRewardInteractables, _weaponRewardInteractables;
    [SerializeField] WaveReward[] _statRewardPool, _weaponRewardPool;

    int _enemiesRemaining;

    int _waveNumber = 1;

    [SerializeField] int _rewardIncrement = 2; 
    [SerializeField] int _stageMorphIncrement = 5;

    [SerializeField] AnimationCurve _dropCurve, _ascensionCurve;


    private void Start()
    {
        GenerateRewards(_statRewardPool, _statRewardInteractables);
        GenerateRewards(_weaponRewardPool, _weaponRewardInteractables);

        Invoke(nameof(EnableRewardInteractables), 2);
    }

    private void Update()
    {
        if (_enemiesRemaining <= 0)
        {
            WaveClear();
        }
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

            interactables[i].OnRewardClaimedCallback += DisableRewardInteractables;
        }
    }

    void StartTestWave()
    {
        List<GameObject> enemiesToSpawn = GetEnemies(50, WaveType.Balanced);

        foreach (GameObject g in enemiesToSpawn)
        {
            SpawnEnemy(Instantiate(g).transform);
        }
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        
        // Spawn enemy --> Animate it going from water to island --> Enable enemy AI.
    }

    void DisableRewardInteractables()
    {
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            StartCoroutine(AscendReward(_statRewardInteractables[i], Random.Range(0, 0.4f)));

        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            StartCoroutine(AscendReward(_weaponRewardInteractables[i], Random.Range(0, 0.4f)));
    }

    void EnableRewardInteractables()
    {
        for (int i = 0; i < _statRewardInteractables.Length; i++)
            StartCoroutine(DropReward(_statRewardInteractables[i], Random.Range(0, 0.4f)));

        for (int i = 0; i < _weaponRewardInteractables.Length; i++)
            StartCoroutine(DropReward(_weaponRewardInteractables[i], Random.Range(0, 0.4f)));
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

    List<GameObject> GetEnemies(int totalBudget, WaveType waveType)
    {
        List<GameObject> finalEnemyList = new();

        int remainingBudget = totalBudget;

        switch (waveType)
        {
            case WaveType.Random:
                for ( ; ; )
                {
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_waveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    if (remainingBudget < 2)
                        break;

                    finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
                    remainingBudget -= randomEnemyCost;
                }
                break;

            case WaveType.Light:
                for ( ; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_waveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 60% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.40f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
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
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
                            remainingBudget -= randomEnemyCost;

                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;

            case WaveType.Balanced:
                for ( ; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_waveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 40% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.60f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
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
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
                            remainingBudget -= randomEnemyCost;

                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;

            case WaveType.Heavy:
                for ( ; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_waveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 10% of budget on enemies with a cost lower than 10% of the budget
                    if (remainingBudget >= totalBudget * 0.90f)
                    {
                        if (randomEnemyCost <= totalBudget * 0.1f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
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
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
                            remainingBudget -= randomEnemyCost;


                            if (remainingBudget < totalBudget * 0.1f)
                                break;
                        }
                    }
                }
                break;

            case WaveType.Swarm:
                for ( ; ; )
                {
                    // Get random enemy
                    int randomEnemyID = Random.Range(0, _enemyAssortment.Length);

                    if (_waveNumber < _enemyAssortment[randomEnemyID].WaveUnlocked)
                        continue;

                    int randomEnemyCost = _enemyAssortment[randomEnemyID].Cost;

                    // Spend 100% of budget on enemies with a cost lower than 15% of the budget
                    if (remainingBudget >= 0)
                    {
                        if (randomEnemyCost <= totalBudget * 0.15f)
                        {
                            finalEnemyList.Add(_enemyAssortment[randomEnemyID].EnemyPrefab);
                            remainingBudget -= randomEnemyCost;

                            if (remainingBudget < 2)
                                break;
                        }
                    }
                }
                break;

            case WaveType.Boss:
                EnemyPreset wealthiestEnemy = _enemyAssortment[0];

                foreach (EnemyPreset e in _enemyAssortment)
                {
                    if (e.Cost > wealthiestEnemy.Cost && _waveNumber >= e.WaveUnlocked)
                        wealthiestEnemy = e;
                }

                finalEnemyList.Add(wealthiestEnemy.EnemyPrefab);
                break;
        }

        return finalEnemyList;
    }

    void WaveClear()
    {
        _waveNumber++;

        // Give rewards
    }

    enum WaveType
    { 
        Random,
        Light,
        Balanced,
        Heavy,
        Swarm,
        Boss
    }
}

[System.Serializable]
public class EnemyPreset
{
    public string EnemyName;

    public GameObject EnemyPrefab;

    [Tooltip("How expensive this unit is, the wave system gets a budget and spends it on units based on their cost.")]
    public int Cost;

    [Tooltip("The wave at which this enemy can start spawning")]
    public int WaveUnlocked;
}
