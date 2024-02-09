using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStateContext
{
	public WaveStateContext(WaveStateMachine stateMachine)
	{
        WaveNumber = 1;
        StateMachine = stateMachine;
	}

	// Creation State
	public WaveStateMachine StateMachine { get; private set; }

    public int WaveNumber { get; private set; }

    public EnemyPreset[] EnemiesToSpawn { get; private set; }

    public void SetEnemiesToSpawn(List<EnemyPreset> enemiesList)
    {
        EnemiesToSpawn = enemiesList.ToArray();
    }

    public void NextWave()
	{
        EnemiesToSpawn = null;
        WaveNumber++;
	}
}
