using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction = Vector2.down;

    [SerializeField]
    private float fallSpeed = 5;

    private void FixedUpdate()
    {
        transform.position += direction * fallSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Target target = collision.gameObject.GetComponent<Target>();
            target.HandleHit();
        }
        Destroy(gameObject);
    }
}
