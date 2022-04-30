using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public event System.Action<GameObject> OnCollected;
    protected virtual void OnCollect()
    {
        if(OnCollected != null)
        {
            OnCollected(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnCollect();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
