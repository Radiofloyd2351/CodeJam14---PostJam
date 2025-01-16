using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Destroy(collision.gameObject);

        if (collision.gameObject.CompareTag("Arrow") && gameObject.CompareTag("Target"))
        {
            Destroy(gameObject);
            Progression.Open(Level.Hell);
        }
    }
}
