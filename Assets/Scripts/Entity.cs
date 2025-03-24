using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using FMODUnity;
using FMOD.Studio;
using System;

public delegate IEnumerator EntityPlayerEventHandler(Entity target);
public abstract class Entity : MonoBehaviour {

    public event EntityPlayerEventHandler OnCollision;
    public event EntityPlayerEventHandler OnTriggerEnter;
    public event EntityPlayerEventHandler OnTriggerExit;


    public string baseSoundDir;

    [SerializeField] private Animator _animator;

    [SerializeField] public int id;

    public BarObject healthBar;
    [SerializeField] private GameObject _staminaBar;
    [SerializeField] private GameObject _staminaContainer;


    public BarObject staminaBar;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _healthContainer;

    public AbsPlayerMovementAbility moveAbility = null;
    public string soundBank;

    [SerializeField] public float speed;

    private Rigidbody2D _body;
    public Rigidbody2D Body {  get { return _body; } }

    private void Awake() {
        healthBar = new BarObject(this, 4100, Color.red, _healthBar, _healthContainer, 100f, 0.3f, 10f);
        staminaBar = new BarObject(this, 4000, Color.green, _staminaBar, _staminaContainer);
        _body = GetComponent<Rigidbody2D>();
    }


    public EventInstance? PlaySound<T>(string sound, KeyValuePair<string, T>? parameterPairs = null) {
        if (!EventManager.IsInitialized) {
            return null;
        }
        EventInstance soundEvent = RuntimeManager.CreateInstance(EventReference.Find(sound));
        RuntimeManager.AttachInstanceToGameObject(soundEvent, gameObject);
        if (parameterPairs != null) {
            if (typeof(T) == typeof(string)) {
                soundEvent.setParameterByNameWithLabel(parameterPairs.Value.Key, parameterPairs.Value.Value.ToString());
            }
            else {
                soundEvent.setParameterByName(parameterPairs.Value.Key, Convert.ToSingle(parameterPairs.Value.Value));
            }
        }
        soundEvent.start();
        return soundEvent;
    }

    public void PlaySoundComplex<T>(string sound, KeyValuePair<string, T>[] parameterPairs = null) {
        if (!EventManager.IsInitialized) {
            return;
        }
        EventInstance soundEvent = RuntimeManager.CreateInstance(EventReference.Find(sound));
        RuntimeManager.AttachInstanceToGameObject(soundEvent, gameObject);
        if (parameterPairs != null) {
            foreach (KeyValuePair<string, T> pair in parameterPairs) {
                if (typeof(T) == typeof(string)) {
                    soundEvent.setParameterByNameWithLabel(pair.Key, pair.Value.ToString());
                }
                else {
                    soundEvent.setParameterByName(pair.Key, Convert.ToSingle(pair.Value));
                }
            }
        }
        soundEvent.start();
    }

    public void RunMoveAnim(Vector2 velocity) {
        _animator.SetBool("IsMoving", true);
        _animator.SetFloat("x", velocity.x);
        _animator.SetFloat("y", velocity.y);
    }

    public void RunDashAnim(Vector2 velocity) {
        _animator.SetTrigger("Dash");
        _animator.SetFloat("x", velocity.x);
        _animator.SetFloat("y", velocity.y);
    }

    public void RunMoveAnim(Direction direction) {
        _animator.SetBool("IsMoving", true);
        _animator.SetInteger("Direction", (int)direction);
    }

    public void StopMoveAnim() {
        _animator.SetBool("IsMoving", false);
    }

    public void StopDashAnim() {
        _animator.SetTrigger("FinishDash");
    }

    public virtual IEnumerator ResetControls() { yield return null; }

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

    /*private void OnCollisionEnter2D(Collision2D collider) { 
        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity != null) {
            RunCollision(entity);
        }
    }*/

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
