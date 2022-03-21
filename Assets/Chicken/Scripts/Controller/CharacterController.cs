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

    public static Action<GameObject> OnShootProjectile;

    public void Init(Vector3 position)
    {
        transform.position = position;
        visualAxis.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void Flap()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        Debug.Log("Flap");

        OnShootProjectile?.Invoke(projectilePrefab);
    }
}
