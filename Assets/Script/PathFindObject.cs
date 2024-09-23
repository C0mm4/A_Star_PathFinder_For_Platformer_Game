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
        if (IsObjectOnTilemap())
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
                            UnityEngine.Debug.Log("Path: " + step);
                            tilemap.SetTile(new Vector3Int(step.x + tilemap.cellBounds.xMin, step.y + tilemap.cellBounds.yMin, 0), pathTile);
                        }
                        UnityEngine.Debug.Log("End Searching");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("can't find explore path.");
                    }
                    UnityEngine.Debug.Log($"Pathfinding execute time : {stopwatch.ElapsedMilliseconds} ms");
                    findPath = true;
                }
            }
        }
    }

    bool IsObjectOnTilemap()
    {
        // Translate world position to Cell position of tilemap
        Vector3Int cellPosition = currentMap.mapTile.WorldToCell(transform.position);

        // Get Cell bounds of tilemap
        BoundsInt bounds = currentMap.mapTile.cellBounds;

        // Check position into cell bounds
        if (!bounds.Contains(cellPosition))
        {
            return false;
        }

        // if object position in cell bounds, and cell position hasn't any tile, return true
        TileBase tile = currentMap.mapTile.GetTile(cellPosition);
        tileMapPos = new Vector2Int(cellPosition.x, cellPosition.y);
        if (tile == null)
        {
            return true;
        }

        // if cell position has any tile, return false
        return false;
    }


}