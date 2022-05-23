using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Tile Type", menuName = "PiRunner/Mapping/Map Tile Type")]
public class MapTileType : ScriptableObject
{
    public string AreaName;
    public List<GameObject> PossibleRewardsPrefabs;
    public float SpawnRewardProbability = 0.2f;
    public GameObject GetRandomReward()
    {
        if (PossibleRewardsPrefabs.Count == 0)
            return null;

        return PossibleRewardsPrefabs[Random.Range(0, PossibleRewardsPrefabs.Count)];
    }
}
