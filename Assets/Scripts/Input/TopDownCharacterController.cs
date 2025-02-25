using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using Movement;

public class TopDownCharacterController : MonoBehaviour {
    #region Attributes
    [SerializeField]
    List<Instrument> instrumentNames;
    public float speed;
    private Characters _controls;
    Rigidbody2D body;
    public AbsPlayerMovementAbility moveAbility = null;
    Vector2 direction = Vector2.zero;

    private bool isPressed = false;
    private bool isReleased = false;

    public FMODUnity.EventReference walkRef;
    private FMOD.Studio.EventInstance walkSound;
    #endregion

    private void Start()
    {
        // TESTING
        moveAbility = gameObject.AddComponent<Dash>();
        // END TEST


        walkSound = FMODUnity.RuntimeManager.CreateInstance(walkRef);

        _controls = new Characters();
        _controls.Enable();

        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingPerform;
        _controls.BasicActions.Movement.canceled += MovementHandlingDisable;

        _controls.BasicActions.Interact.performed += InteractionHandling;

        _controls.BasicActions.SpecialMove.performed += SpecialMovementAbilityExecute;

        _controls.BasicActions.InstrumentSwitch.performed += InstrumentSwitching;

        body = GetComponent<Rigidbody2D>();
    }

    void MovementHandlingPerform(InputAction.CallbackContext ctx) {

        if (body != null) {
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
            //PlayWalkSound();
            // TODO: Add Back the animations and the sounds functionality.
            DefaultValues.player.GetComponent<PlayerAnims>().RunAnim(body.velocity);
        }
    }

    void MovementHandlingEnable(InputAction.CallbackContext ctx) {
        isReleased = false;
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * speed;
            if (!isPressed) {
                isPressed = true;
                StartCoroutine(ChangeVelocity(ctx));
            }
        }
    }
    
    IEnumerator ChangeVelocity(InputAction.CallbackContext ctx) {
        while(!isReleased) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * speed;
            yield return new WaitForFixedUpdate();
        }
        isPressed = false;
    }

    void MovementHandlingDisable(InputAction.CallbackContext ctx) {
        if (body != null) {
            direction = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            body.velocity = direction * speed;
            isReleased = true;
            DefaultValues.player.GetComponent<PlayerAnims>().StopAnim();
        }
        
    }

    void InteractionHandling(InputAction.CallbackContext ctx) {
        EventHandler.instance.RunInterraction();
    }


    void SpecialMovementAbilityExecute(InputAction.CallbackContext ctx) {
        _controls.BasicActions.Movement.Disable();
        Debug.Log("Triggered Special Ability");
        moveAbility.Move(body, direction);
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

