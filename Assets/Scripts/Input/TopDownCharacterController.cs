using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class TopDownCharacterController : MonoBehaviour {
    #region Attributes
    [SerializeField]
    List<Instrument> instrumentNames;
    public float speed;
    private Characters _controls;
    Rigidbody2D body;


    private bool canPlayWalkSound = true;

    private bool isWalking = true;

    
    private bool isPressed = false;
    private bool isReleased = false;

    public FMODUnity.EventReference walkRef;
    private FMOD.Studio.EventInstance walkSound;
    #endregion

    private void Start()
    {
        walkSound = FMODUnity.RuntimeManager.CreateInstance(walkRef);

        _controls = new Characters();
        _controls.Enable();

        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingPerform;
        _controls.BasicActions.Movement.canceled += MovementHandlingDisable;

        _controls.BasicActions.Interact.performed += InteractionHandling;

        _controls.BasicActions.InstrumentSwitch.performed += InstrumentSwitching;

        body = GetComponent<Rigidbody2D>();
    }

    void MovementHandlingPerform(InputAction.CallbackContext ctx) {

        if (body != null) {
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
            PlayWalkSound();
            // TODO: Add Back the animations and the sounds functionality.
            DefaultValues.player.GetComponent<PlayerAnims>().RunAnim(body.velocity);
        }
    }

    void MovementHandlingEnable(InputAction.CallbackContext ctx) {
        isReleased = false;
        if (body != null) {
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
            if (!isPressed) {
                isPressed = true;
                StartCoroutine(ChangeVelocity(ctx));
            }
            PlayWalkSound();
        }
    }

    IEnumerator ChangeVelocity(InputAction.CallbackContext ctx) {
        while(!isReleased) {
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
            yield return new WaitForFixedUpdate();
        }
        isPressed = false;
    }

    void MovementHandlingDisable(InputAction.CallbackContext ctx) {
        if (body != null) {
            body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
            StopWalkSound();
            isReleased = true;
            DefaultValues.player.GetComponent<PlayerAnims>().StopAnim();
        }
        
    }

    void InteractionHandling(InputAction.CallbackContext ctx) {
        EventHandler.instance.RunInterraction();
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

    IEnumerator WalkingSoundRoutine() {
        if (canPlayWalkSound) {
            canPlayWalkSound = false;
            while (isWalking) {
                walkSound.start();
                yield return new WaitForSeconds(0.5f);
            }
            canPlayWalkSound = true;
        }
    }

    void PlayWalkSound() {
        if (!isWalking) {
            isWalking = true;
            StartCoroutine(WalkingSoundRoutine());
        }
    }

    void StopWalkSound() {
        StopCoroutine(WalkingSoundRoutine());
        isWalking = false;
        walkSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}

