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
        ConvertTilemapToGrid();

        aStar = new(mapGridData, mapTile.cellBounds.xMin, mapTile.cellBounds.yMin);
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

}

