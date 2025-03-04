using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public delegate IEnumerator EntityPlayerEventHandler(Entity target);
public abstract class Entity : MonoBehaviour {
    private int _health;

    public event EntityPlayerEventHandler OnCollision;
    public event EntityPlayerEventHandler OnTriggerEnter;
    public event EntityPlayerEventHandler OnTriggerExit;


    [SerializeField] private Animator animator;

    [SerializeField] public int id;

    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject staminaContainer;
    private float stamina;
    private float maxStamina = 10f;
    const int STAMINA_ID = 4000;
    private float stamina_regen_cooldown = 2f;
    private float stamina_regen = 1f;
    private float stamina_bar_size;
    public AbsPlayerMovementAbility moveAbility = null;

    [SerializeField] public float speed;

    private Rigidbody2D _body;
    public Rigidbody2D Body {  get { return _body; } }

    private void Awake() {


        stamina = maxStamina;
        stamina_bar_size = staminaBar.transform.localScale.x;
        _body = GetComponent<Rigidbody2D>();
    }



    public bool PayStamina(float cost) {
        if (stamina < cost) {
            return false;
        }
        staminaContainer.SetActive(true);
        stamina -= cost;
        staminaBar.transform.localPosition = new Vector3(((stamina/maxStamina)-1) * stamina_bar_size / 2f, 0f, 0f);
        staminaBar.transform.localScale = new Vector3(stamina_bar_size * stamina / maxStamina, staminaBar.transform.localScale.y, 0f);
        CoroutineManager.instance.RunCoroutine(ReloadStamina(), STAMINA_ID + id);
        return true;
    }
    public IEnumerator ReloadStamina() {
        yield return new WaitForSeconds(stamina_regen_cooldown);
        float t = 0;
        while (stamina < maxStamina) {
            yield return new WaitForSeconds(1f);
            stamina += stamina_regen;
            if (stamina > maxStamina) {
                stamina = maxStamina;
            }
            staminaBar.transform.localPosition = new Vector3(((stamina / maxStamina) - 1) * stamina_bar_size / 2f, 0f, 0f);
            staminaBar.transform.localScale = new Vector3(stamina_bar_size * stamina / maxStamina, staminaBar.transform.localScale.y, 0f);
        }
        yield return new WaitForSeconds(1f);
        staminaContainer.SetActive(false);
    }


    public float GetStamina() {
        return stamina;
    }

    public void RunAnim(Vector2 velocity) {
        animator.SetBool("IsMoving", true);
        animator.SetFloat("x", velocity.x);
        animator.SetFloat("y", velocity.y);
    }

    public void RunAnim(Direction direction) {
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
