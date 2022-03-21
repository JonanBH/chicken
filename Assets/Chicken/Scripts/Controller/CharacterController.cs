using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float flapForce = 10;
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    public void Flap()
    {
        rigidbody2D.AddForce(Vector2.up, ForceMode2D.Impulse);
    }
}
