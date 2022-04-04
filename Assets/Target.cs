using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float minSpeed = 5;
    private float maxSpeed = 10;
    private float speed = 0;
    [SerializeField]
    private Rigidbody2D rb2d;

    private void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(transform.position + Vector3.left * speed * Time.deltaTime);

        if(transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void HandleHit()
    {
        GameController.singleton.AddScore(5);
        Destroy(gameObject);
    }
}
