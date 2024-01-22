using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
	#region Singleton

	public static TilemapManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("More than one instance of TilemapManager found on " + gameObject.name + ". Destroying Instance");
			Destroy(this);
		}
	}

	#endregion


	[SerializeField] Tilemap _walkableMap;

	public Dictionary<Vector2Int, Tile> Grid;

    private void Start()
    {
		CreateGrid();
    }

    // Creates a grid of the walkable-tilemaps size. Adds coordinates to each of the GridCells
    void CreateGrid()
	{
		Grid = new Dictionary<Vector2Int, Tile>();

		for (int x = _walkableMap.cellBounds.xMin; x < _walkableMap.cellBounds.xMax; x++)
		{
			for (int y = _walkableMap.cellBounds.yMin; y < _walkableMap.cellBounds.yMax; y++)
			{
				Vector2Int localPos = new Vector2Int(x, y);

				if (_walkableMap.HasTile((Vector3Int)localPos))
				{
					if (!Grid.ContainsKey(localPos))
					{
						Tile gridCell = gameObject.AddComponent<Tile>();
						gridCell.GridLocation = localPos;

						Grid.Add(localPos, gridCell);
					}
				}
				else
                {
					Tile gridCell = gameObject.AddComponent<Tile>();
					gridCell.GridLocation = localPos;
					gridCell.IsBlocked = true;

					Grid.Add(localPos, gridCell);
				}


			}
		}
	}

	public Vector2 GridToWorldPos(Vector2Int gridPos)
	{
		return _walkableMap.CellToWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
	}

	public Vector2Int WorldToGridPos(Vector2 worldPos)
	{
		return (Vector2Int)_walkableMap.WorldToCell(worldPos + new Vector2(0, -0.5f));
	}

	public bool HasWalkableTile(Vector2Int gridPos)
	{
		return Grid.ContainsKey(gridPos) && !Grid[gridPos].IsBlocked;
	}
}
