using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator EntityPlayerEventHandler(Entity target);
public abstract class Entity : MonoBehaviour {
    private int _health;

    public event EntityPlayerEventHandler OnCollision;
    public event EntityPlayerEventHandler OnTriggerEnter;
    public event EntityPlayerEventHandler OnTriggerExit;


    [SerializeField] private Animator animator;

    [SerializeField] public int id;

    [SerializeField] public float speed;

    private Rigidbody2D _body;
    public Rigidbody2D Body {  get { return _body; } }

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
    }

    public void RunAnim(Vector2 velocity) {
        Debug.Log("running anim (velo)!");
        animator.SetBool("IsMoving", true);
        animator.SetFloat("x", velocity.x);
        animator.SetFloat("y", velocity.y);
    }

    public void RunAnim(Direction direction) {
        Debug.Log("running anim!");
        animator.SetBool("IsMoving", true);
        animator.SetInteger("Direction", (int)direction);
    }

    public void StopAnims() {
        animator.SetBool("IsMoving", false);
    }

    public virtual Vector2 GetDirection() { return new Vector2(); }
    public virtual void SetDirection(Vector2 newDir) { }

    public virtual Vector2 GetLastDirection() { return new Vector2(); }
    public virtual void SetLastDirection(Vector2 newDir) { }

    public virtual void EnableMovement() { }
    public virtual void DisableMovement() { }
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

    private void OnCollisionEnter2D(Collision2D collider) { 
        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity != null) {
            RunCollision(entity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity != null) {
            RunTriggerEnter(entity);
        }
    }
    private void OnTriggerExit2D(Collider2D collider) {
        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity != null) {
            RunTriggerExit(entity);
        }
    }
}
