using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    protected virtual void OnTouched()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController player = collision.gameObject.GetComponent<CharacterController>();
            player.TakeDamage();

            OnTouched();
        }
    }
}
