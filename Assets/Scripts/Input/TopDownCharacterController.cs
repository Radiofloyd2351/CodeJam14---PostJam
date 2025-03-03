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
    public AbsPlayerMovementAbility moveAbility = null;
    Vector2 direction = Vector2.zero;
    public Vector2 Direction { get { return direction; } }
    public Vector2 LastDirection = Vector2.zero;

    private bool isPressed = false;
    private bool isReleased = false;

    public FMODUnity.EventReference walkRef;
    private FMOD.Studio.EventInstance walkSound;

    // FOR MESSING AROUND ONLY
    public int maxDashesForMove = 0;
    #endregion

    private void Start()
    {

        stats = gameObject.GetComponent<PlayerStats>();
        // TESTING

         moveAbility = new ChainDash();
        // moveAbility = new SlowDown(true);
        // END TEST


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
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * stats.speed;
            if (body.velocity.magnitude != 0) {
                LastDirection = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            }
            //PlayWalkSound();
            // TODO: Add Back the animations and the sounds functionality.
            DefaultValues.player.GetComponent<PlayerAnims>().RunAnim(body.velocity);
        }
    }

    void MovementHandlingEnable(InputAction.CallbackContext ctx) {
        isReleased = false;
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * stats.speed;
            if (!isPressed) {
                isPressed = true;
                CoroutineManager.instance.RunCoroutine(ChangeVelocity(ctx), COROUTINE_ID_CHANGE_VELOCITY);
            }
        }
    }
    
    IEnumerator ChangeVelocity(InputAction.CallbackContext ctx) {
        while(!isReleased) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * stats.speed;
            yield return new WaitForFixedUpdate();
        }
        isPressed = false;
    }

    void MovementHandlingDisable(InputAction.CallbackContext ctx) {
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * stats.speed;
            isReleased = true;
            DefaultValues.player.GetComponent<PlayerAnims>().StopAnim();
        }
        
    }

    void InteractionHandling(InputAction.CallbackContext ctx) {
        EventHandler.instance.RunInterraction();
    }


    void SpecialMovementAbilityExecute(InputAction.CallbackContext ctx) {
        Debug.Log("Triggered Special Ability");
        moveAbility.Move(stats);
    }

    void SpecialMovementAbilityCancel(InputAction.CallbackContext ctx) {
        Debug.Log("Special Ability disabled");
        moveAbility.Cancel(stats);
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
    public void PlayWalkSound() {
        walkSound.start();
    }

    public void DisableControls() {
        _controls.Disable();
    }
    public void EnableControls() {
        _controls.Enable();
    }
}

