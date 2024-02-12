using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStateContext
{
	public WaveStateContext(WaveStateMachine stateMachine, Transform[] spawnPoints)
	{
        WaveNumber = 1;
        StateMachine = stateMachine;
        SpawnPoints = spawnPoints;
	}

	// Creation State
	public WaveStateMachine StateMachine { get; private set; }

    public int WaveNumber { get; private set; }

    public EnemyPreset[] EnemiesToSpawn { get; private set; }

    public Transform[] SpawnPoints { get; private set; }

    public void SetEnemiesToSpawn(List<EnemyPreset> enemiesList)
    {
        EnemiesToSpawn = enemiesList.ToArray();
    }

    public void SetSpawnPoints(Transform[] spawnPoints)
    {
        SpawnPoints = spawnPoints;
    }

    public void NextWave()
	{
        Debug.Log("CALLING NEXTWAVE");
        EnemiesToSpawn = null;
        WaveNumber++;
	}
}
