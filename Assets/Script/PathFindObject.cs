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
                            UnityEngine.Debug.Log("경로: " + step);
                            tilemap.SetTile(new Vector3Int(step.x + tilemap.cellBounds.xMin, step.y + tilemap.cellBounds.yMin, 0), pathTile);
                        }
                        UnityEngine.Debug.Log("End Searching");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("경로를 찾지 못했습니다.");
                    }
                    UnityEngine.Debug.Log($"Pathfinding execute time : {stopwatch.ElapsedMilliseconds} ms");
                    findPath = true;
                }
            }
        }
    }

    bool IsObjectOnTilemap(Tilemap tilemap, Vector3 worldPosition)
    {
        // 월드 좌표를 타일맵의 셀 좌표로 변환
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // 타일맵의 셀 범위 가져오기
        BoundsInt bounds = tilemap.cellBounds;

        // 좌표가 타일맵의 범위 내에 있는지 확인
        if (!bounds.Contains(cellPosition))
        {
            return false;
        }

        // 셀 범위 내에 있더라도 타일이 없을 수 있으므로 타일 존재 여부 확인
        TileBase tile = tilemap.GetTile(cellPosition);
        tileMapPos = new Vector2Int(cellPosition.x, cellPosition.y);
        if (tile == null)
        {
            return true;
        }

        // 타일이 존재하는 경우
        return true;
    }


}