using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using Movement;

public class TopDownCharacterController : MonoBehaviour {
    const int COROUTINE_ID_CHANGE_VELOCITY = 50000;
    #region Attributes
    [SerializeField]
    List<Instrument> instrumentNames;
    public PlayerStats stats;
    private Characters _controls;
    private Rigidbody2D body;
    public int id = 0; // temporary, switch to entity
    public Rigidbody2D Body { get { return body; } }
    Vector2 direction = Vector2.zero;
    public Vector2 Direction { get { return direction; } }
    public Vector2 LastDirection = Vector2.zero;

    private bool isFrozen = false;

    private bool isPressed = false;
    private bool isReleased = false;

    public FMODUnity.EventReference walkRef;
    private FMOD.Studio.EventInstance walkSound;

    // FOR MESSING AROUND ONLY
    public int maxDashesForMove = 0;
    public float speed = 15;
    #endregion

    private void Start()
    {

        stats = gameObject.GetComponent<PlayerStats>();
        // TESTING


        walkSound = FMODUnity.RuntimeManager.CreateInstance(walkRef);

        _controls = new Characters();
        _controls.Enable();

        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingPerform;
        _controls.BasicActions.Movement.canceled += MovementHandlingDisable;

        _controls.BasicActions.Interact.performed += InteractionHandling;

        _controls.BasicActions.SpecialMove.performed += SpecialMovementAbilityExecute;
        _controls.BasicActions.SpecialMove.canceled += SpecialMovementAbilityCancel;

        _controls.BasicActions.InstrumentSwitch.performed += InstrumentSwitching;

        body = GetComponent<Rigidbody2D>();
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
            // TODO: Add Back the animations and the sounds functionality.
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
        Instrument currInstrument = Instrument.None;
        if(ctx.ReadValue<Vector2>().y  > 0){
            currInstrument = instrumentNames[0];
            InstrumentManager.instance.GetIndicator(Instrument.None).Click();
        }
        if (ctx.ReadValue<Vector2>().x > 0){
            currInstrument = instrumentNames[1];
            InstrumentManager.instance.GetIndicator(Instrument.Launch).Click();
        }
        if (ctx.ReadValue<Vector2>().y < 0){
            currInstrument = instrumentNames[2];
            InstrumentManager.instance.GetIndicator(Instrument.Lyre).Click();
        }
        if (ctx.ReadValue<Vector2>().x < 0){
            currInstrument = instrumentNames[3];
            InstrumentManager.instance.GetIndicator(Instrument.Guitar).Click();
        }
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
        /*_controls.BasicActions.Movement.started -= MovementHandlingEnable;
        _controls.BasicActions.Movement.performed -= MovementHandlingPerform;*/
    }

    public void EnableMovement() {
        isFrozen = false;
        /*
        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingPerform;*/
        if (isPressed) {
            stats.StopMoveAnim();
            stats.RunMoveAnim(LastDirection);
        }
    }
}

