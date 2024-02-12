using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    public GameObject[] islandPrefabs;

    public IslandTransition previousIsland;

    public void GenerateIsland()
    {
        GameObject randomIsland = islandPrefabs[Random.Range(0, islandPrefabs.Length)];
        
        Transform island = Instantiate(randomIsland, new Vector3(-18, 0, 0), Quaternion.identity,transform).transform;
        island.position = new Vector3(-18, 0, 0);

        IslandTransition transition = island.GetComponent<IslandTransition>();

        transition.SwapIsland();
        previousIsland.SwapIsland();
        previousIsland = transition;
    }
}
