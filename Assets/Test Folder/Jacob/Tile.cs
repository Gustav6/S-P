using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Distance from the start of a path to this tile.
    public int StartDistance;

    // Distance from the end of a path to this tile.
    public int EndDistance;

    // Total distance between the target tile in the path. with this we can check what tile is the closest to the end.
    public int Total { get { return StartDistance + EndDistance; } }

    public bool IsBlocked;

    // PreviousTile is used for tracing back when we have found the final path.
    public Tile PreviousTile;

    public Vector2Int GridLocation;
}
