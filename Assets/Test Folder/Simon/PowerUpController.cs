using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;

    private void Start()
    {
        if (EquipmentManager.Instance.OnSpawnPowerUp == null)
            EquipmentManager.Instance.OnSpawnPowerUp = ChanceToSpawnPowerUp;
    }

    private void ChanceToSpawnPowerUp(Vector2 spawnPosition, float chance, PowerUpTypes type)
    {
        if (powerUpPrefab == null)
            return;

        float rng = UnityEngine.Random.Range(0, 101);

        if (rng > chance || chance == 0)
            return;

        GameObject newSpawn;

        if (type == PowerUpTypes.Anything)
        {
            int index = UnityEngine.Random.Range(0, (int)PowerUpTypes.Anything);
            newSpawn = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

            try
            {
                newSpawn.GetComponent<PowerUpActivation>().thisType = (PowerUpTypes)index;
            }
            catch
            {
                Debug.LogError("No PowerUpActivation component found on powerUpPrefab.");
            }
        }
        else
        {
            newSpawn = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

            try
            {
                newSpawn.GetComponent<PowerUpActivation>().thisType = type;
            }
            catch
            {
                Debug.LogError("No PowerUpActivation component found on powerUpPrefab.");
            }
        }

        //newSpawn.GetComponent<PowerUp>().powerUpSprite;
    }
}

public enum PowerUpTypes
{
    Dash,
    Haste,
    Tank,
    Anything
}
