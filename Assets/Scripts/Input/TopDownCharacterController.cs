using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

//TODO: SET UP ACTION WHEEL FOR EXPANSION
public class TopDownCharacterController : MonoBehaviour {
    [SerializeField]
    List<Instrument> instrumentNames;
    public float speed;

    private Dictionary<Instrument, bool> instrumentSwitchDict = new();
    private Characters _controls;
    Rigidbody2D body;
    private Animator animator;
    private bool interacting;

    private bool isWalking = true;

    public FMODUnity.EventReference walkRef;
    private FMOD.Studio.EventInstance walkSound;


    private void Start()
    {
        walkSound = FMODUnity.RuntimeManager.CreateInstance(walkRef);

        animator = GetComponent<Animator>();
        _controls = new Characters();
        _controls.Enable();

        _controls.BasicActions.Movement.started += MovementHandlingEnable;
        _controls.BasicActions.Movement.performed += MovementHandlingEnable;
        _controls.BasicActions.Movement.canceled += MovementHandlingDisable;

        _controls.BasicActions.Interact.performed += InteractionHandling;

        _controls.BasicActions.InstrumentSwitch.performed += InstrumentSwitching;

        body = GetComponent<Rigidbody2D>();

        for(int i = 0; i < instrumentNames.Count; i++){
            instrumentSwitchDict.Add(instrumentNames[i], false);
        }

    }

    void MovementHandlingEnable(InputAction.CallbackContext ctx) {
        body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
        PlayWalkSound();
        // TODO: Add Back the animations and the sounds functionality.
    }

    void MovementHandlingDisable(InputAction.CallbackContext ctx) {
        body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
        StopWalkSound();
        // TODO: Add Back the animations and the sounds functionality.
    }

    void InteractionHandling(InputAction.CallbackContext ctx) {
        interacting = _controls.BasicActions.Interact.triggered;
        Debug.Log("INTERACTED" + " " + interacting);
        // TODO: implement logic for interaction.
    }

    void InstrumentSwitching(InputAction.CallbackContext ctx) {
        Instrument currInstrument = Instrument.None;
        if(ctx.ReadValue<Vector2>().y  > 0){
            currInstrument = instrumentNames[0];
            DefaultValues.instrumentsInfo[Instrument.None].indicator.Click();
        }
        if (ctx.ReadValue<Vector2>().x > 0){
            currInstrument = instrumentNames[1];
            DefaultValues.instrumentsInfo[Instrument.Launch].indicator.Click();
        }
        if (ctx.ReadValue<Vector2>().y < 0){
            currInstrument = instrumentNames[2];
            DefaultValues.instrumentsInfo[Instrument.Lyre].indicator.Click();
        }
        if (ctx.ReadValue<Vector2>().x < 0){
            currInstrument = instrumentNames[3];
            DefaultValues.instrumentsInfo[Instrument.Guitar].indicator.Click();
        }
    }

    IEnumerator WalkingSoundRoutine() {
        yield return new WaitForSeconds(0.1f);
        while (isWalking) {
            walkSound.start();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void PlayWalkSound() {
        if (!isWalking) {
            Debug.Log("WALKING");
            isWalking = true;
            StartCoroutine(WalkingSoundRoutine());
        }
    }

    void StopWalkSound() {
        Debug.Log("NOT WALKING");
        StopCoroutine(WalkingSoundRoutine());
        isWalking = false;
        walkSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Update()
    {
        /*
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;

        if (!walkingSound && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))) {
            walkingSound = true;
            StartCoroutine(WalkingSound());
        }

        if (walkingSound && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))) {
            walkingSound = false;
        }
        */
    }

}

