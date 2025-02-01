using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector2 lastVelocity = new Vector2(0f,0f);

    public void RunAnim(Vector2 velocity) {

        //animator.SetTrigger("Debounce");
        animator.SetBool("IsMoving", true);
        Debug.Log(velocity + "pen is");
        if (velocity.y > 0) {
            if (velocity.x < 0) {
                animator.SetInteger("Direction", 7);
            } else if (velocity.x > 0) {
                animator.SetInteger("Direction", 4);
            } else {
                animator.SetInteger("Direction", 1);
            }
        } else if (velocity.y < 0) {
            if (velocity.x < 0) {
                animator.SetInteger("Direction", 6);
            } else if (velocity.x > 0) {
                animator.SetInteger("Direction", 5);
            } else {
                animator.SetInteger("Direction", 0);
            }
        } else {
            if (velocity.x < 0) {
                animator.SetInteger("Direction", 3);
            } else if (velocity.x > 0) {
                animator.SetInteger("Direction", 2);
            } else {
            }
        }
    }

    public void StopAnim() {
        //animator.SetTrigger("Debounce");
        animator.SetBool("IsMoving", false);
    } 
}
