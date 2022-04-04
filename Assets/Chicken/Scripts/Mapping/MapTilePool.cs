using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTilePool : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private float preheatPoolAmount = 5;
    [SerializeField]
    private List<MapTileGroup> prefabsByTileType = new List<MapTileGroup>();
    private Dictionary<MapTileType, List<MapTile>> mapTilePools = new Dictionary<MapTileType, List<MapTile>>();
    public static MapTilePool Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Requere da pool do tipo desejado, reaquecendo a pool caso seus itens cheguem a 0
    /// </summary>
    /// <param name="tileType"></param>
    /// <returns></returns>
    public MapTile GetMapTile(MapTileType tileType)
    {
        if(!mapTilePools.TryGetValue(tileType, out List<MapTile> pool))
        {
            pool = new List<MapTile>();
            for(int i = 0; i < preheatPoolAmount; i++)
            {
                pool.Add(GenerateTileOfType(tileType));
            }
            mapTilePools.Add(tileType, pool);
        }
        else
        {
            if(pool.Count == 0)
            {
                for(int i = 0; i < preheatPoolAmount; i++)
                {
                    pool.Add(GenerateTileOfType(tileType));
                }
            }
        }
        MapTile selectedTile = pool[Random.Range(0, pool.Count)];
        pool.Remove(selectedTile);
        selectedTile.transform.parent = null;

        return selectedTile;
    }

    /// <summary>
    /// Devolve o tile de mapa para a pool
    /// </summary>
    /// <param name="mapTile"></param>
    public void ReturnMapTileToPool(MapTile mapTile)
    {
        mapTile.gameObject.SetActive(false);
        mapTilePools[mapTile.GetMapTileType()].Add(mapTile);
        mapTile.transform.parent = transform;
    }

    /// <summary>
    /// Cria uma nova instância do maptile baseado na pool do tipo
    /// </summary>
    /// <param name="mapTileType"></param>
    /// <returns></returns>
    private MapTile GenerateTileOfType(MapTileType mapTileType)
    {
        List<GameObject> prefabOptions = GetPrefabsByMapTileType(mapTileType);
        GameObject targetPrefab = prefabOptions[Random.Range(0, prefabOptions.Count)];

        MapTile newTile = Instantiate(targetPrefab).GetComponent<MapTile>();
        newTile.transform.parent = transform;
        newTile.gameObject.SetActive(false);
        return newTile;
    }

    /// <summary>
    /// Get prefab list using the type received
    /// </summary>
    /// <param name="mapTileType"></param>
    /// <returns></returns>
    private List<GameObject> GetPrefabsByMapTileType(MapTileType mapTileType)
    {
        foreach(MapTileGroup group in prefabsByTileType)
        {
            if(group.mapTileType == mapTileType)
            {
                return group.prefabs;
            }
        }
        return null;
    }


}
