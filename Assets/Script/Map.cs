using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Map : MonoBehaviour
{

    public Tilemap mapTile;
    public int[,] mapGridData;

    public AStarPathfinding aStar;
    [SerializeField]
    public List<int> grid;


    public void Start()
    {

        UpdateBounds();
        ConvertTilemapToGrid();

        aStar = new(mapGridData, mapTile.cellBounds.xMin, mapTile.cellBounds.yMin);
        grid = ConvertToList(mapGridData);
    }

    public List<int> ConvertToList(int[,] array)
    {
        List<int> list = new List<int>();

        // 다차원 배열의 크기 얻기
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        // 각 요소를 리스트에 추가
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                list.Add(array[i, j]);
            }
        }

        return list;
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

