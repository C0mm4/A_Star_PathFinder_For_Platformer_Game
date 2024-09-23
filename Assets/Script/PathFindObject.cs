using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindObject : MonoBehaviour
{
    public Map currentMap;
    [SerializeField]
    public Vector2Int tileMapPos;

    public PathFindObject targetObject;

    public bool findPath = true;

    public Tilemap tilemap;
    public RuleTile pathTile;
    public int jumpF;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        Vector3 objectPosition = transform.position;
        if (IsObjectOnTilemap(currentMap.mapTile, objectPosition))
        {
            if (targetObject != null)
            {
                if (!findPath)
                {
                    UnityEngine.Debug.Log("Start Searching");
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var astarSearch = currentMap.aStar.FindPathInField(tileMapPos, targetObject.tileMapPos, jumpF);
                    var path = astarSearch.Result;
                    stopwatch.Stop();
                    if (path.Count > 0)
                    {
                        foreach (Vector2Int step in path)
                        {
                            UnityEngine.Debug.Log("���: " + step);
                            tilemap.SetTile(new Vector3Int(step.x + tilemap.cellBounds.xMin, step.y + tilemap.cellBounds.yMin, 0), pathTile);
                        }
                        UnityEngine.Debug.Log("End Searching");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("��θ� ã�� ���߽��ϴ�.");
                    }
                    UnityEngine.Debug.Log($"Pathfinding execute time : {stopwatch.ElapsedMilliseconds} ms");
                    findPath = true;
                }
            }
        }
    }

    bool IsObjectOnTilemap(Tilemap tilemap, Vector3 worldPosition)
    {
        // ���� ��ǥ�� Ÿ�ϸ��� �� ��ǥ�� ��ȯ
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Ÿ�ϸ��� �� ���� ��������
        BoundsInt bounds = tilemap.cellBounds;

        // ��ǥ�� Ÿ�ϸ��� ���� ���� �ִ��� Ȯ��
        if (!bounds.Contains(cellPosition))
        {
            return false;
        }

        // �� ���� ���� �ִ��� Ÿ���� ���� �� �����Ƿ� Ÿ�� ���� ���� Ȯ��
        TileBase tile = tilemap.GetTile(cellPosition);
        tileMapPos = new Vector2Int(cellPosition.x, cellPosition.y);
        if (tile == null)
        {
            return true;
        }

        // Ÿ���� �����ϴ� ���
        return true;
    }


}