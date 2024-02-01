using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] EnemyPreset[] _enemyAssortment;
    [SerializeField] List<Transform> _spawnPoints;

    int _enemiesRemaining;

    int _waveNumber = 1;

    [SerializeField] int _rewardIncrement = 2; 
    [SerializeField] int _stageMorphIncrement = 5;

    [SerializeField] AnimationCurve _horizontalCurve, _verticalCurve;

    private void Start()
    {
        StartTestWave();
    }

    private void Update()
    {
        if (_enemiesRemaining <= 0)
        {
            WaveClear();
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
        StartCoroutine(DispenseEnemy(enemy.transform, randomSpawnPoint.GetChild(0).position, randomSpawnPoint.GetChild(1).position));
    }

    // Enemy gets dispensed out of the ocean, onto the island
    IEnumerator DispenseEnemy(Transform enemyTransform, Vector2 startPos, Vector2 endPos)
    {
        float time = 0;

        while (time < 1)
        {
            enemyTransform.position = new Vector2(Mathf.Lerp(startPos.x, endPos.x, time), Mathf.Lerp(startPos.y, endPos.y, _verticalCurve.Evaluate(time)));
            time += Time.deltaTime;
            yield return null;
        }

        enemyTransform.position = endPos;
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

                    if (remainingBudget <= 2)
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

                            if (remainingBudget <= 2)
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

                            if (remainingBudget <= 2)
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

                            if (remainingBudget <= 2)
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

                            if (remainingBudget <= 2)
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

    [Tooltip("How expensive this unit is, the wave system gets a budget and spends it on units.")]
    public int Cost;

    [Tooltip("The wave at which this enemy can start spawning")]
    public int WaveUnlocked;
}
