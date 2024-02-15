using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpActivation powerUpPrefab;
    [SerializeField] private AnimationCurve _verticalCurve;

    public Transform SpawnPointsParent;

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

        PowerUpActivation newSpawn;

        if (type == PowerUpTypes.Anything)
        {
            int index = UnityEngine.Random.Range(0, (int)PowerUpTypes.Anything);
            newSpawn = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

            try
            {
                newSpawn.thisType = (PowerUpTypes)index;
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
                newSpawn.thisType = type;
            }
            catch
            {
                Debug.LogError("No PowerUpActivation component found on powerUpPrefab.");
            }
        }

        KeyValuePair<Transform, float> closestSpawnPoint = new(null, 1000);

        List<Transform> spawnPoints = new();

        foreach (Transform g in SpawnPointsParent.transform)
        {  // This will only find direct children
            Transform t = g.gameObject.GetComponent<Transform>();
            spawnPoints.Add(t);
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            float distance = (spawnPosition - (Vector2)spawnPoints[i].GetChild(1).position).magnitude;

            if (distance < closestSpawnPoint.Value)
                closestSpawnPoint = new(spawnPoints[i], distance);
        }

        StartCoroutine(AnimatePowerUpSpawn(newSpawn, spawnPosition, closestSpawnPoint.Key.GetChild(1).position));
        EquipmentManager.Instance.PowerUpSpawned(newSpawn.gameObject);
    }

    // Property of Jacob
    IEnumerator AnimatePowerUpSpawn(PowerUpActivation powerUp, Vector2 startPos, Vector2 endPos)
    {
        powerUp.enabled = false;
        Transform parentTransform = new GameObject().transform;
        powerUp.transform.position = parentTransform.position;
        powerUp.transform.SetParent(parentTransform);

        parentTransform.position = startPos;

        float time = 0;

        while (time <= 1.25f)
        {
            yield return null;
            time += Time.deltaTime;
            parentTransform.position = Vector2.Lerp(startPos, endPos, time / 1.25f);
            powerUp.transform.localPosition = new Vector2(0, Mathf.Lerp(0, 1.5f, _verticalCurve.Evaluate(time / 1.25f)));
        }

        powerUp.transform.SetParent(null);
        powerUp.enabled = true;
        Destroy(parentTransform.gameObject);
    }
}

public enum PowerUpTypes
{
    Dash,
    Haste,
    Tank,
    Heal,
    Anything
}
