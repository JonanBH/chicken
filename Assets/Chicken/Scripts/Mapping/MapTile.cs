using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [SerializeField]
    private Transform rightConnector;
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private MapTileType mapTileType;
    [SerializeField]
    private List<Transform> rewardSpawnPositions = new List<Transform>();
    private List<GameObject> spawnedRewards = new List<GameObject>();
    private Quaternion currentRotation;
    private bool hasSwipeSucceeded = false;

    /// <summary>
    /// Reseta o componente para poder ser usado novamente
    /// </summary>
    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        currentRotation = Quaternion.identity;

        foreach(GameObject reward in spawnedRewards)
        {
            if(reward != null)
                Destroy(reward);
        }

        spawnedRewards.Clear();
    }

    public void SpawnRewards()
    {
        if (rewardSpawnPositions.Count == 0)
            return;

        float rng = UnityEngine.Random.value;
        if(rng <= mapTileType.SpawnRewardProbability)
        {
            int randomIndex = UnityEngine.Random.Range(0, spawnedRewards.Count);

            GameObject reward = Instantiate(mapTileType.GetRandomReward());
            reward.transform.SetParent(transform);
            reward.transform.position = rewardSpawnPositions[randomIndex].position;

            spawnedRewards.Add(reward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject);
        if (!other.CompareTag("Player")) return;

        MapGenerator.Instance.GenerateNext();
        MapGenerator.Instance.FreeTilesBehind(this);
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (!GetIsTurning()) return;

        if (!other.TryGetComponent<PlayerController>(out PlayerController player)) { return; }

        if (hasSwipeSucceeded) return;

        HandleSwipeFailed();
    }*/


    /// <summary>
    /// Get the right anchor connection of the tile
    /// </summary>
    /// <returns></returns>
    public Transform GetRightConnector()
    {
        return rightConnector;
    }

    public MapTileType GetMapTileType()
    {
        return mapTileType;
    }

    public Vector3 GetOffsetVector()
    {
        return (transform.position - anchor.position);
    }

    public Transform GetAnchor()
    {
        return anchor;
    }

    public void Move(Vector3 movement)
    {
        transform.position += movement;
    }
}
