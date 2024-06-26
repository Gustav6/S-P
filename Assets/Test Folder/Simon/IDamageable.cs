using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Simon
public interface IDamageable
{
    public float KnockbackPercent { get; set; }
    public int ConsecutiveHits { get; set; }

    /// <summary>
    /// Gets all tiles that can be stood upon.
    /// </summary>
    /// <param name="tilemap">The current maps tilemap</param>
    /// <returns>All the tiles in a tilemap in a grid like dictionary</returns>
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
        AudioManager.Instance.PlaySound("Hurt");
        KnockbackPercent += damageAmount;
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration);

    public virtual void TakeKnockback(Vector2 sourcePosition, Vector2 targetDirection, float knockbackMultiplier, float stunDuration)
    {
        // Defined in each individual enemy.
    }

    /// <summary>
    /// Checks if the entity is standing on a tile.
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="tiles">All tiles of the tilemap in a dictionary, use PopulateTilesDictonary method</param>
    /// <param name="collider">The collider of the entity that checks if the entity is standing on a valid tile</param>
    public void CheckDeath(Tilemap tilemap, Dictionary<Vector2Int, TileBase> tiles, Collider2D collider)
    {
        Vector2 position = collider.bounds.center;
        Vector2 size = collider.bounds.size;

        Vector2 center = tilemap.CellToWorld(new Vector3Int(0, 0)) + Vector3.up * 0.37f; // Add 0.37 to account for grid offset.
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
            Die();
        }
    }

    public void Die();
}
