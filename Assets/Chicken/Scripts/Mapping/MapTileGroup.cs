using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Tile Group", menuName = "PiRunner/Mapping/Map Tile Group")]
public class MapTileGroup : ScriptableObject
{
    public MapTileType mapTileType;
    public List<GameObject> prefabs;
}
