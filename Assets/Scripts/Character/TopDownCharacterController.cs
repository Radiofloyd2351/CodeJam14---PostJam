using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TopDownCharacterController : MonoBehaviour
{
    public float speed;

    private bool walkingSound;

    private Characters _movement;
    public AudioSource source;
    public AudioClip[] clip;
    Rigidbody2D body;
    private Animator animator;
    private bool interacting;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _movement = new Characters();
        _movement.Enable();
        _movement.BasicMovement.Movement.started += movementHandling;
        _movement.BasicMovement.Movement.performed += movementHandling;
        _movement.BasicMovement.Movement.canceled += movementHandling;
        _movement.BasicMovement.Interact.started += interactionHandling;
        _movement.BasicMovement.Interact.performed += interactionHandling;
        _movement.BasicMovement.Interact.canceled += interactionHandling;
        body = GetComponent<Rigidbody2D>();
    }

    void movementHandling(InputAction.CallbackContext ctx) {
        body.velocity = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1) * speed;
        // TODO: Add Back the animations and the sounds functionality.
    }

    void interactionHandling(InputAction.CallbackContext ctx) {
        interacting = _movement.BasicMovement.Interact.triggered;
        Debug.Log("INTERACTED");
        // TODO: implement logic for interaction.
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


    private IEnumerator WalkingSound() {
        int x;
        while (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) {
            x = Random.Range(0, 4);
            int pitch = Random.Range(9,12);
            float pitch2 = (float)(pitch) / 10f;
            source.pitch = pitch2;


            int volume = Random.Range(8, 10);
            float volume2 = (float)(volume) / 1000f;

            source.volume = volume2;

            source.PlayOneShot(clip[x]);

            int wait = Random.Range(40, 50);
            float wait2 = (float)(wait) / 100f;


            yield return new WaitForSeconds(wait2);
        }
    }
}

