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
    private Quaternion currentRotation;
    private bool hasSwipeSucceeded = false;

    /// <summary>
    /// Reseta o componente para poder ser usado novamente
    /// </summary>
    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        currentRotation = Quaternion.identity;
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
