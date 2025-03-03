using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator EntityPlayerEventHandler(Entity target);
public abstract class Entity : MonoBehaviour {
    private int _health;

    public event EntityPlayerEventHandler OnCollision;
    public event EntityPlayerEventHandler OnTriggerEnter;
    public event EntityPlayerEventHandler OnTriggerExit;

    [SerializeField] public int id;

    public bool RunCollision(Entity target) {
        if (OnCollision != null) {
            foreach (EntityPlayerEventHandler handler in OnCollision.GetInvocationList()) {
                StartCoroutine(handler(target));
            }
        }
        return false;
    }

    public bool RunTriggerEnter(Entity target) {
        if (OnTriggerEnter != null) {
            foreach (EntityPlayerEventHandler handler in OnTriggerEnter.GetInvocationList()) {
                StartCoroutine(handler(target));
            }
            return true;
        }
        return false;
    }

    public bool RunTriggerExit(Entity target) {
        if (OnTriggerExit != null) {
            foreach (EntityPlayerEventHandler handler in OnTriggerExit.GetInvocationList()) {
                StartCoroutine(handler(target));
            }
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Entity>() != null) {
            RunCollision(collision.gameObject.GetComponent<Entity>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log(collider);
        if (collider.gameObject.GetComponent<Entity>() != null) {
            RunTriggerEnter(collider.gameObject.GetComponent<Entity>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider) {
        Debug.Log(collider);
        if (collider.gameObject.GetComponent<Entity>() != null) {
            RunTriggerExit(collider.gameObject.GetComponent<Entity>());
        }
    }
}
