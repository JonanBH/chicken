using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private MapTileType currentMapTileType;
    [SerializeField]
    private float maxDistanceToKeep = 30f;
    [SerializeField]
    private int startingPlataforms = 10;
    [SerializeField]
    private Transform tilesContainer;
    private int remainingCurves = 3;
    private float nextCurveIn;
    private List<MapTile> mapTiles = new List<MapTile>();
    private bool isGenerateRight = false;
    private bool isGeneratingCurve = false;
    private int remainingCurveSize = 0;
    private long nextPiExponent = 0;
    public static MapGenerator Instance;

    private void Awake() 
    {    
        Instance = this;
    }

    private void Start() 
    {
        MapTile tile = MapTilePool.Instance.GetMapTile(currentMapTileType);
        tile.transform.position = Vector3.zero;
        tile.gameObject.SetActive(true);
        tile.transform.parent = tilesContainer;
        mapTiles.Add(tile);

        for (int i = 0; i < startingPlataforms; i++)
        {
            GenerateNext();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateNext();
        }
    }

    public void GenerateNext()
    {
        MapTile tile;
        tile = GenerateNextTile();
        mapTiles.Add(tile);
    }

    private MapTile GenerateNextTile()
    {
        MapTile tile = MapTilePool.Instance.GetMapTile(currentMapTileType);
        tile.Reset();
        Transform connector = mapTiles[mapTiles.Count - 1].GetRightConnector();
        tile.transform.position = connector.position + tile.transform.right * tile.GetOffsetVector().magnitude;
        tile.gameObject.SetActive(true);
        tile.transform.parent = tilesContainer;
        //mapTiles.Add(tile);

        return tile;
    }

    public void MoveMap(Vector3 movement)
    {
        foreach (MapTile tile in mapTiles)
        {
            tile.Move(movement);
        }
    }

    public void FreeTilesBehind(MapTile mapTile)
    {
        int position = mapTiles.IndexOf(mapTile) - 1;
        if (position < 0) return;

        while( position >= 0) 
        {
            MapTile current = mapTiles[0];
            mapTiles.Remove(current);
            MapTilePool.Instance.ReturnMapTileToPool(current);
            position--;
        }
    }

    
}

