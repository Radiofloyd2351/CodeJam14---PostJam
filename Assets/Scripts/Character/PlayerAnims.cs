using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void RunAnim(Vector2 velocity) {
        animator.SetBool("IsMoving", true);
        Debug.Log(velocity + "pen is");
        if (velocity.y > 0) {
            animator.SetInteger("Direction", 1);
        } else if (velocity.y < 0) {
            animator.SetInteger("Direction", 0);
        }

        if (velocity.x > 0) {
            animator.SetInteger("Direction", 2);
        } else if (velocity.x < 0) {
            animator.SetInteger("Direction", 3);
        }
    }

    public void RunAnim(int direction) {
        animator.SetBool("IsMoving", true);
        animator.SetInteger("Direction", direction);
    }

    public void StopAnim() {
        animator.SetBool("IsMoving", false);
    } 
}
