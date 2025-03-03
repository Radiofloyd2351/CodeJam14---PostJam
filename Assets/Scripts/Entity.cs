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

    [SerializeField] public float speed;

    public virtual Vector2 GetDirection() { return new Vector2(); }
    public virtual void SetDirection(Vector2 newDir) { }

    public virtual Vector2 GetLastDirection() { return new Vector2(); }
    public virtual void SetLastDirection(Vector2 newDir) { }

    public virtual void EnableControls() { }
    public virtual void DisableControls() { }
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
        //Debug.Log(collider);
        if (collider.gameObject.GetComponent<Entity>() != null) {
            RunTriggerEnter(collider.gameObject.GetComponent<Entity>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider) {
        //Debug.Log(collider);
        if (collider.gameObject.GetComponent<Entity>() != null) {
            RunTriggerExit(collider.gameObject.GetComponent<Entity>());
        }
    }
}
