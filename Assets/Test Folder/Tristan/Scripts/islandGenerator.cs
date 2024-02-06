using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class islandGenerator : MonoBehaviour
{
    public GameObject islandRoundPrefab;
    public GameObject islandDonutPrefab;

    int islandNumber;
    void Start()
    {
        GenerateIsland();
    }

    void Update()
    {
    }

    public void GenerateIsland()
    {
        islandNumber = Random.Range(0, 2);
        
        if (islandNumber == 0) {
            Transform island = Instantiate(islandRoundPrefab, new Vector3(-18, 0, 0), Quaternion.identity,transform).transform;
            island.position = new Vector3(-18, 0, 0);
        }
        else if (islandNumber != 0) {
            Instantiate(islandDonutPrefab, new Vector3(-18, 0, 0), Quaternion.identity,transform);
        }
    }
}
