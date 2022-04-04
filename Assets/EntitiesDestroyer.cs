using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("cols");
        Destroy(collision.gameObject);
    }
}
