using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IDamageable
{
    public float KnockbackPercent { get; set; }

    public static Dictionary<Vector2Int, TileBase> PopulateTilesDictonary(Tilemap tilemap)
    {
        Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

        for (int i = tilemap.cellBounds.xMin; i <= tilemap.cellBounds.xMax; i++)
        {
            for (int j = tilemap.cellBounds.yMin; j <= tilemap.cellBounds.yMax; j++)
            {
                tiles.Add(new Vector2Int(i, j), tilemap.GetTile(new Vector3Int(i, j)));
            }
        }

        return tiles;
    }

    public void TakeDamage(float damageAmount)
    {
        KnockbackPercent += damageAmount;
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration);

    public void CheckDeath(Tilemap tilemap, Dictionary<Vector2Int, TileBase> tiles, Vector2 position, Vector2 size)
    {
        // Add 0.37 to account for grid offset.
        Vector2 center = tilemap.CellToWorld(new Vector3Int(0, 0)) + Vector3.up * 0.37f;
        Vector2 direction = position - center;
        direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));

        Vector2 positionToCheck = (position - (direction * (size / 2))) - Vector2.up * 0.37f; // Add 0.37 here for the same reason.

        Vector3Int cellPosition = tilemap.WorldToCell(positionToCheck);

        try
        {
            if (tilemap.ContainsTile(tiles[new Vector2Int(cellPosition.x, cellPosition.y)]))
                return;
            else
            {
                Die();
            }
        }
        catch
        {
            Debug.Log("Outside of bounds.");
            Die();
        }
    }

    public void Die();
}
