using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStateContext
{
	public WaveStateContext(WaveStateMachine stateMachine, Transform spawnPoints, Transform respawnPoint)
	{
        WaveNumber = 1;
        StateMachine = stateMachine;
        SpawnPointParent = spawnPoints;
        RespawnPoint = respawnPoint;
	}

	// Creation State
	public WaveStateMachine StateMachine { get; private set; }

    public int WaveNumber { get; private set; }

    public EnemyPreset[] EnemiesToSpawn { get; private set; }

    public Transform SpawnPointParent { get; private set; }

    public Transform RespawnPoint { get; private set; }

    public void SetEnemiesToSpawn(List<EnemyPreset> enemiesList)
    {
        EnemiesToSpawn = enemiesList.ToArray();
    }

    public void SetSpawnPointsParent(Transform spawnPointsParent)
    {
        SpawnPointParent = spawnPointsParent;
    }

    public void SetRespawnPoint(Transform respawnPoint)
    {
        RespawnPoint = respawnPoint;
    }

    public void NextWave()
	{
        EnemiesToSpawn = null;
        WaveNumber++;
	}
}
