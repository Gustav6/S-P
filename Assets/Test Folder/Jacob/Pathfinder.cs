using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
	bool CanSeeTarget(Vector2 currentPos, Vector2 targetPos)
    {
		Vector2 targetDirection = targetPos - currentPos;

		Ray ray = new(currentPos, targetDirection);

        for (int i = 0; i < 5; i++)
        {
			if (!TilemapManager.Instance.HasWalkableTile(TilemapManager.Instance.WorldToGridPos(ray.GetPoint(0.4f * i))))
				return false;
        }

		return true;
    }

	public Vector2 FindPath(Tile startTile, Tile targetTile, Vector2 targetPos)
	{
		// Potential tiles that fit all the criteria and can be checked
		List<Tile> suitableTiles = new();

		List<Tile> checkedTiles = new();

		suitableTiles.Add(startTile);

		while (suitableTiles.Count > 0)
		{
			// Sets the current tile to be checked to whatever tile has the highest "value" (More about this can be seen in Tile.cs)
			Tile currentTile = suitableTiles.OrderBy(x => x.Total).First();

			suitableTiles.Remove(currentTile);
			checkedTiles.Add(currentTile);

			if (CanSeeTarget(TilemapManager.Instance.GridToWorldPos(currentTile.GridLocation), targetPos))
				return TilemapManager.Instance.GridToWorldPos(currentTile.GridLocation);

			List<Tile> neighboringTiles = GetNeighboringTiles(currentTile, startTile.GridLocation, targetTile.GridLocation);

			foreach (Tile neighbor in neighboringTiles)
			{
				if (neighbor.IsBlocked || checkedTiles.Contains(neighbor))
					continue;

				neighbor.StartDistance = GetManhattanDistance(startTile, neighbor);
				neighbor.EndDistance = GetManhattanDistance(targetTile, neighbor);

				neighbor.PreviousTile = currentTile;

				if (!suitableTiles.Contains(neighbor))
					suitableTiles.Add(neighbor);
			}
		}

		return Vector2.one * 100000000;
	}

	// Gets the distance between two tiles in total moves needed (note, only the four cardinal directions are counted)
	int GetManhattanDistance(Tile startTile, Tile neighbor)
	{
		return Mathf.Abs(startTile.GridLocation.x - neighbor.GridLocation.x) + Mathf.Abs(startTile.GridLocation.y - neighbor.GridLocation.y);
	}

	List<Tile> GetNeighboringTiles(Tile currentTile, Vector2Int startPos, Vector2Int targetPos)
	{
		var grid = TilemapManager.Instance.Grid;

		List<Tile> neighbors = new();

		Vector2Int targetDirection = targetPos - startPos;

		// Whole workaround to prevent them from being 0, and instead clamping to either -1 or 1. There's probably a better solution but this works for now.
		// In the case that either of these are 0, the player is prevented from pathfinding around objects on the same axis.
		int targetX = (int)((Mathf.Clamp(targetDirection.x, 0f, 0.2f) - 0.1f) * 10f);
		int targetY = (int)((Mathf.Clamp(targetDirection.y, 0f, 0.2f) - 0.1f) * 10f);

		// Loop to declutter the code, "i" is used for reversing the direction we are checking
		for (int i = 1; i >= -1; i -= 2)
		{
			// Makes sure we prioritize pathing in whatever direction you are furthest away from the target tile in.
			Vector2Int locationToCheck = Mathf.Abs(targetDirection.x) >= Mathf.Abs(targetDirection.y) ?
				new Vector2Int(
				currentTile.GridLocation.x,
				currentTile.GridLocation.y + targetY * i
				) :
				new Vector2Int(
				currentTile.GridLocation.x + targetX * i,
				currentTile.GridLocation.y
				);

			// If there is a tile chosen position, 
			if (grid.ContainsKey(locationToCheck))
			{
				neighbors.Add(grid[locationToCheck]);
			}


			// Same thing, just makes sure we also check the y/x axis
			locationToCheck = Mathf.Abs(targetDirection.x) >= Mathf.Abs(targetDirection.y) ?
				new Vector2Int(
				currentTile.GridLocation.x + targetX * i,
				currentTile.GridLocation.y
				) :
				new Vector2Int(
				currentTile.GridLocation.x,
				currentTile.GridLocation.y + targetY * i
				);

			if (grid.ContainsKey(locationToCheck))
			{
				neighbors.Add(grid[locationToCheck]);
			}
		}

		return neighbors;
	}
}
