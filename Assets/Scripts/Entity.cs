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

    [SerializeField] private GameObject _staminaBar;
    [SerializeField] private GameObject _staminaContainer;
    private float _stamina;
    private float _maxStamina = 100f;
    const int STAMINA_ID = 4000;
    private float _stamina_regen_cooldown = 2f;
    private float _stamina_regen = 1f;
    private float _stamina_bar_size;


    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _healthContainer;
    private float _health;
    private float _maxHealth = 100f;
    const int HEALTH_ID = 6000;
    private float _health_regen_cooldown = 2f;
    private float _health_regen = 1f;
    private float _health_bar_size;

    public AbsPlayerMovementAbility moveAbility = null;
    public string soundBank;

    [SerializeField] public float speed;

    private Rigidbody2D _body;
    public Rigidbody2D Body {  get { return _body; } }

    private void Awake() {


        _stamina = _maxStamina;
        _stamina_bar_size = _staminaBar.transform.localScale.x;
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

    public bool PayStamina(float cost) {
        if (_stamina < cost) {
            return false;
        }
        _staminaContainer.SetActive(true);
        _stamina -= cost;
        _staminaBar.transform.localPosition = new Vector3(((_stamina/_maxStamina)-1) * _stamina_bar_size / 2f, 0f, 0f);
        _staminaBar.transform.localScale = new Vector3(_stamina_bar_size * _stamina / _maxStamina, _staminaBar.transform.localScale.y, 0f);
        CoroutineManager.instance.RunCoroutine(ReloadStamina(), STAMINA_ID + id);
        return true;
    }
    public IEnumerator ReloadStamina() {
        yield return new WaitForSeconds(_stamina_regen_cooldown);
        while (_stamina < _maxStamina) {
            yield return new WaitForSeconds(0.01f);
            _stamina += _stamina_regen;
            if (_stamina > _maxStamina) {
                _stamina = _maxStamina;
            }
            _staminaBar.transform.localPosition = new Vector3(((_stamina / _maxStamina) - 1) * _stamina_bar_size / 2f, 0f, 0f);
            _staminaBar.transform.localScale = new Vector3(_stamina_bar_size * _stamina / _maxStamina, _staminaBar.transform.localScale.y, 0f);
        }
        yield return new WaitForSeconds(1f);
        _staminaContainer.SetActive(false);
    }


    public float GetStamina() {
        return _stamina;
    }


    public bool PayHEalth(float cost) {
        if (_health < cost) {
            return false;
        }
        _staminaContainer.SetActive(true);
        _stamina -= cost;
        _staminaBar.transform.localPosition = new Vector3(((_health / _maxHealth) - 1) * _health_bar_size / 2f, 0f, 0f);
        _staminaBar.transform.localScale = new Vector3(_health_bar_size * _stamina / _maxHealth, _staminaBar.transform.localScale.y, 0f);
        CoroutineManager.instance.RunCoroutine(ReloadHealth(), HEALTH_ID + id);
        return true;
    }
    public IEnumerator ReloadHealth() {
        yield return new WaitForSeconds(_stamina_regen_cooldown);
        while (_stamina < _maxStamina) {
            yield return new WaitForSeconds(0.01f);
            _stamina += _stamina_regen;
            if (_stamina > _maxStamina) {
                _stamina = _maxStamina;
            }
            _staminaBar.transform.localPosition = new Vector3(((_stamina / _maxStamina) - 1) * _stamina_bar_size / 2f, 0f, 0f);
            _staminaBar.transform.localScale = new Vector3(_stamina_bar_size * _stamina / _maxStamina, _staminaBar.transform.localScale.y, 0f);
        }
        yield return new WaitForSeconds(1f);
        _staminaContainer.SetActive(false);
    }


    public float GetHealth() {
        return _stamina;
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
