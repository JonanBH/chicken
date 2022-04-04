using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float flapForce = 10;
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    [SerializeField]
    private Transform visualAxis;
    [SerializeField]
    private GameObject projectilePrefab;
    private bool flyBlocked = false;
    [SerializeField]
    private float blockedTime = 1;


    public static Action<GameObject> OnShootProjectile;

    public void Init(Vector3 position)
    {
        transform.position = position;
        visualAxis.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if(transform.position.y >= GameController.singleton.MaxHeigth && flyBlocked == false)
        {
            flyBlocked = true;
            Invoke("UnlockFly", blockedTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void UnlockFly()
    {
        flyBlocked = false;
    }

    public void Flap()
    {
        if (flyBlocked) return;

        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        Debug.Log("Flap");

        OnShootProjectile?.Invoke(projectilePrefab);
    }
}
