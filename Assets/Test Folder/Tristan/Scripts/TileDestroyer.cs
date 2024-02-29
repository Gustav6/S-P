using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    [SerializeField] Tile GrassTopMid, GrassCurveBotLeft, GrassCurveBotRight, GrassCurveTopLeft, GrassCurveTopRight, FullGrass, GrassMidLeft, GrassMidRight, GrassBotRight, GrassBotLeft, GrassBotMid, GrassTopLeft, GrassTopRight;

    enum Tile_Type {
        Unknown, GrassTopMid, GrassTopRight, GrassTopLeft, GrassBotMid, GrassBotLeft, GrassBotRight, GrassMidRight, GrassMidLeft, FullGrass, GrassCurveTopRight, GrassCurveTopLeft, GrassCurveBotRight, GrassCurveBotLeft,
    }

    enum Tile_Location {
        N, W, S, E, NE, NW, SW, SE
    }

    List<object> localTiles;
    List<Tile_Type> localTileTypes;

    float tileHealth = 100;
    Tilemap tilemap;
    Vector3 mousePos;
    Vector3Int snappedTile, tilePos;
    
    [SerializeField] GridLayout grid;

    Vector3Int nTilePos, wTilePos, sTilePos, eTilePos, neTilePos, nwTilePos, swTilePos, seTilePos;

    Tile nTile, wTile, sTile, eTile, neTile, nwTile, seTile, swTile;
    void Start() {
        tilemap = GetComponent<Tilemap>();
        grid = GetComponentInParent<Grid>();
    }

    void Update()
    {
        tilemap = GetComponent<Tilemap>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            DestroyTile(mousePos, 50);
        }
    }

    Tile_Type GetTile_Type(Tile tile) {
        if (tile == GrassTopMid) { return Tile_Type.GrassTopMid; }
        if (tile == GrassTopRight) { return Tile_Type.GrassTopRight; }
        if (tile == GrassTopLeft) { return Tile_Type.GrassTopLeft; }
        if (tile == GrassBotMid) { return Tile_Type.GrassBotMid; }
        if (tile == GrassBotRight) { return Tile_Type.GrassBotRight; }
        if (tile == GrassBotLeft) { return Tile_Type.GrassBotLeft; }
        if (tile == GrassMidRight) { return Tile_Type.GrassMidRight; }
        if (tile == GrassMidLeft) { return Tile_Type.GrassMidLeft; }
        if (tile == FullGrass) { return Tile_Type.FullGrass; }
        if (tile == GrassCurveTopRight) { return Tile_Type.GrassCurveTopRight; }
        if (tile == GrassCurveTopLeft) { return Tile_Type.GrassCurveTopLeft; }
        if (tile == GrassCurveBotRight) { return Tile_Type.GrassCurveBotRight; }
        if (tile == GrassCurveBotLeft) { return Tile_Type.GrassCurveBotLeft; }
        return Tile_Type.Unknown;
    }

    void GetLocalTiles(Vector3Int snappedTile) {
        nTilePos = new Vector3Int(snappedTile.x, snappedTile.y + 1, 0);
        sTilePos = new Vector3Int(snappedTile.x, snappedTile.y - 1, 0);
        wTilePos = new Vector3Int(snappedTile.x - 1, snappedTile.y, 0);
        eTilePos = new Vector3Int(snappedTile.x + 1, snappedTile.y, 0);
        neTilePos = new Vector3Int(snappedTile.x + 1, snappedTile.y + 1, 0);
        swTilePos = new Vector3Int(snappedTile.x - 1, snappedTile.y - 1, 0);
        nwTilePos = new Vector3Int(snappedTile.x - 1, snappedTile.y + 1, 0);
        seTilePos = new Vector3Int(snappedTile.x + 1, snappedTile.y - 1, 0);

        nTile = tilemap.GetTile<Tile>(nTilePos);
        wTile = tilemap.GetTile<Tile>(wTilePos);
        sTile = tilemap.GetTile<Tile>(sTilePos);
        eTile = tilemap.GetTile<Tile>(eTilePos);
        neTile = tilemap.GetTile<Tile>(neTilePos);
        nwTile = tilemap.GetTile<Tile>(nwTilePos);
        swTile = tilemap.GetTile<Tile>(swTilePos);
        seTile = tilemap.GetTile<Tile>(seTilePos);
        localTiles = new List<object> { nTile, wTile, sTile, eTile, neTile, nwTile, swTile, seTile};
        localTileTypes = new List<Tile_Type> { GetTile_Type(nTile), GetTile_Type(wTile), GetTile_Type(sTile), GetTile_Type(eTile), GetTile_Type(neTile), GetTile_Type(nwTile), GetTile_Type(swTile), GetTile_Type(seTile)};
    }

    void UpdateTile(Tile_Location loc) {
        Debug.Log("Updating");
        // Checks (almost) all possible cases and places the correct tile for said case
        switch (loc)
        {
            case Tile_Location.N:
                switch (localTileTypes[0])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(nTilePos, null);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(nTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(nTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(nTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(nTilePos, GrassBotRight);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(nTilePos, GrassTopLeft);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(nTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(nTilePos, null);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(nTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(nTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(nTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(nTilePos, GrassBotLeft);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(nTilePos, GrassBotRight);
                        break;
                }
                break;
            case Tile_Location.W:
                switch (localTileTypes[1])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(wTilePos, GrassTopRight);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(wTilePos, GrassBotRight);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(wTilePos, GrassBotRight);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(wTilePos, GrassBotLeft);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(wTilePos, GrassMidRight);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(wTilePos, null);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(wTilePos, GrassMidRight);
                        break;
                }
                break;
            case Tile_Location.S:
                switch (localTileTypes[2])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(sTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(sTilePos, GrassTopRight);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(sTilePos, GrassTopLeft);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(sTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(sTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(sTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(sTilePos, null);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(sTilePos, null);
                        break;
                }
                break;
            case Tile_Location.E:
                switch (localTileTypes[3])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(eTilePos, GrassTopLeft);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(eTilePos, GrassBotLeft);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(eTilePos, GrassMidLeft);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(eTilePos, null);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(eTilePos, GrassMidLeft);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(eTilePos, null);
                        break;
                }
                break;
            case Tile_Location.NE:
                switch (localTileTypes[4])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(neTilePos, GrassTopLeft);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(neTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(neTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(neTilePos, GrassCurveBotLeft);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(neTilePos, null);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(neTilePos, null);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(neTilePos, GrassCurveBotLeft);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(neTilePos, null);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(neTilePos, GrassMidLeft);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(neTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(neTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(neTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(neTilePos, null);
                        break;
                }
                break;
            case Tile_Location.NW:
                switch (localTileTypes[5])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(nwTilePos, GrassTopRight);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(nwTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(nwTilePos, GrassCurveBotRight);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(nwTilePos, GrassMidRight);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(nwTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(nwTilePos, GrassBotMid);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(nwTilePos, null);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(nwTilePos, GrassMidRight);
                        break;
                }
                break;
            case Tile_Location.SW:
                switch (localTileTypes[6])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(swTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(swTilePos, GrassTopRight);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(swTilePos, GrassTopLeft);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(swTilePos, null);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(swTilePos, GrassBotRight);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(swTilePos, GrassBotLeft);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(swTilePos, GrassCurveTopRight);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(swTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(swTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(swTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(swTilePos, null);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(swTilePos, null);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(swTilePos, GrassMidRight);
                        break;
                }
                break;
            case Tile_Location.SE:
                switch (localTileTypes[7])
                {
                    case Tile_Type.GrassTopMid:
                        tilemap.SetTile(seTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassTopRight:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassTopLeft:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassBotMid:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassBotRight:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassBotLeft:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.FullGrass:
                        tilemap.SetTile(seTilePos, GrassCurveTopLeft);
                        break;
                    case Tile_Type.GrassCurveTopRight:
                        tilemap.SetTile(seTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveTopLeft:
                        tilemap.SetTile(seTilePos, GrassTopMid);
                        break;
                    case Tile_Type.GrassCurveBotRight:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassCurveBotLeft:
                        tilemap.SetTile(seTilePos, null);
                        break;
                    case Tile_Type.GrassMidLeft:
                        tilemap.SetTile(seTilePos, GrassMidLeft);
                        break;
                    case Tile_Type.GrassMidRight:
                        tilemap.SetTile(seTilePos, null);
                        break;
                }
                break;
        }
    }

    void SetLocalTiles() {
        Debug.Log("Setting");
        foreach (Tile_Location loc in Tile_Location.GetValues(typeof(Tile_Location))) {
            Debug.Log("Locating");
            UpdateTile(loc);
        }

        
        if (tilemap.GetComponent<TilemapRenderer>().mode == TilemapRenderer.Mode.Individual)
        {
            tilemap.GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Chunk;
        }
        else
        {
            tilemap.GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Individual;
        } 
    }

    void DestroyTile(Vector3 worldPos, float dmg) {
        Debug.Log("I'm in");
        tileHealth -= dmg;
        tilePos = grid.WorldToCell(worldPos);
        snappedTile = new Vector3Int(tilePos.x, tilePos.y, 0);
        // Fix it to be on Z-0 so it works
        if (tileHealth <= 0) {
            GetLocalTiles(snappedTile);
            
            if (tilemap != null && tilemap.HasTile(snappedTile)) {
                Debug.Log("It has it");
                tilemap.SetTile(snappedTile, null);
                tilemap.RefreshTile(snappedTile);
                Debug.Log("It has been removed");
            }
            else {
                Debug.Log("Does not have it");
            }
            SetLocalTiles();
            tileHealth = 100;
        }
    }
}
