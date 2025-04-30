using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCharacterController : MonoBehaviour {
    const int COROUTINE_ID_CHANGE_VELOCITY = 50000;
    #region Attributes
    [SerializeField]
    List<Instrument> instrumentNames;
    [SerializeField]
    MenuControls menuControls;
    public PlayerStats stats;
    private Characters _controls;
    private Rigidbody2D body;
    public Rigidbody2D Body { get { return body; } }
    Vector2 direction = Vector2.zero;
    public Vector2 Direction { get { return direction; } }
    public Vector2 LastDirection = Vector2.zero;

    private bool isFrozen = false;

    private bool isPressed = false;
    private bool isReleased = false;
    #endregion

    private void Start()
    {

        stats = gameObject.GetComponent<PlayerStats>();
        // TESTING

        _controls = new Characters();
        _controls.Enable();
        _controls.MenuDiving.Disable();
        menuControls.StartUp(_controls);

        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingPerform;
        _controls.BasicActions.Movement.canceled += MovementHandlingDisable;

        _controls.BasicActions.Interact.performed += InteractionHandling;
        _controls.BasicActions.Attack.performed += Attack;

        _controls.BasicActions.SpecialMove.performed += SpecialMovementAbilityExecute;
        _controls.BasicActions.SpecialMove.canceled += SpecialMovementAbilityCancel;

        _controls.BasicActions.Instrument1.performed += InstrumentSwitching;
        _controls.BasicActions.Instrument2.performed += InstrumentSwitching;
        _controls.BasicActions.Instrument3.performed += InstrumentSwitching;
        _controls.BasicActions.Instrument4.performed += InstrumentSwitching;

        _controls.BasicActions.Menu.performed += ToggleMenu;

        body = GetComponent<Rigidbody2D>();
    }

    void ToggleMenu(InputAction.CallbackContext ctx) {
        Inventory.instance.ToggleMenu();
        _controls.BasicActions.Disable();
        _controls.MenuDiving.Enable();
    }
    void MovementHandlingPerform(InputAction.CallbackContext ctx) {
        if (body != null) {
            if (!isFrozen) {
                body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * stats.speed;
                DefaultValues.playerStats.RunMoveAnim(body.velocity);
            }
            if (body.velocity.magnitude != 0) {
                LastDirection = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            }
        }
    }

    void MovementHandlingEnable(InputAction.CallbackContext ctx) {
        isReleased = false;
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            if (!isFrozen) {
                body.velocity = direction * stats.speed;
            }
            if (!isPressed) {
                isPressed = true;
                CoroutineManager.instance.RunCoroutine(ChangeVelocity(ctx), COROUTINE_ID_CHANGE_VELOCITY);
            }
        }
    }
    
    IEnumerator ChangeVelocity(InputAction.CallbackContext ctx) {

        while (!isReleased) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            if (!isFrozen) {
                body.velocity = direction * stats.speed;
            }
            yield return new WaitForFixedUpdate();
        }
        isPressed = false;
        
    }

    void MovementHandlingDisable(InputAction.CallbackContext ctx) {
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
                body.velocity = direction * stats.speed;
            isReleased = true;
            DefaultValues.playerStats.StopMoveAnim();
        }
        
    }

    void InteractionHandling(InputAction.CallbackContext ctx) {
        EventHandler.instance.RunInterraction();
    }


    void SpecialMovementAbilityExecute(InputAction.CallbackContext ctx) {
        stats.moveAbility.Move(stats);
    }

    void SpecialMovementAbilityCancel(InputAction.CallbackContext ctx) {
        stats.moveAbility.Cancel(stats);
    }

    void InstrumentSwitching(InputAction.CallbackContext ctx) {
        InstrumentFactory.instance.SwitchInstrument((int)ctx.ReadValue<float>());
    }

    void Attack(InputAction.CallbackContext ctx) {
        stats.attackAbility.Attack(stats);
    }

    public void DisableMovementAbility() {
        _controls.BasicActions.SpecialMove.Disable();
    }

    public void DisableControls() {
        _controls.Disable();
    }
    public void EnableControls() {
        _controls.Enable();
    }
    public void DisableMovement() {
        isFrozen = true;
    }

    public void EnableMovement() {
        isFrozen = false;
        if (isPressed) {
            stats.StopMoveAnim();
            stats.RunMoveAnim(LastDirection);
        }
    }
}

