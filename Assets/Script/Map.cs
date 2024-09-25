using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{

    public Tilemap mapTile;
    public int[,] mapGridData;

    public AStarPathfinding aStar;


    public void Start()
    {

        UpdateBounds();
        UpdateTile();
    }


    public void UpdateBounds()
    {
        mapTile.ResizeBounds();
        mapTile.RefreshAllTiles();
    }


    private void ConvertTilemapToGrid()
    {
        BoundsInt bounds = mapTile.cellBounds;
        mapGridData = new int[bounds.size.x, bounds.size.y];
        // converting [-inf, inf] data to [0, inf] data and that position exists any tile, set data 1(can't move area)
        for (int y = bounds.yMax - 1; y >= bounds.yMin; y--)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                TileBase tile = mapTile.GetTile(tilePos);

                if (tile != null)
                {
                    mapGridData[x - bounds.xMin, y - bounds.yMin] = 1;
                }
            }
        }

    }

    public void UpdateTile()
    {
        ConvertTilemapToGrid();
        aStar = new(mapGridData, mapTile.cellBounds.xMin, mapTile.cellBounds.yMin);
    }
}

