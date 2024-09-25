using UnityEngine;
using UnityEngine.Tilemaps;

public class Systems : MonoBehaviour
{
    public Tilemap mapTile;
    public Tilemap pathTile;

    public GameObject Starter;
    public GameObject target;

    public Map map;
    public void ResetButton()
    {
        map.UpdateTile();
        pathTile.ClearAllTiles();
        pathTile.origin = mapTile.origin;
    }

    public void Search()
    {
        ResetButton();
        Starter.GetComponent<PathFindObject>().findPath = false;
    }
}
