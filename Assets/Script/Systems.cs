using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Systems : MonoBehaviour
{
    public Tilemap mapTile;
    public Tilemap pathTile;

    public GameObject Starter;
    public GameObject target;
    public void ResetButton()
    {
        pathTile.ClearAllTiles();
        pathTile.origin = mapTile.origin;
    }

    public void Search()
    {
        Starter.GetComponent<PathFindObject>().findPath = false;
    }
}
