using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void RunAnim(Vector2 velocity) {
        animator.SetBool("IsMoving", true);
        if (velocity.y > 0) {
            animator.SetInteger("Direction", 0);
        } else if (velocity.y < 0) {
            animator.SetInteger("Direction", 1);
        }

        if (velocity.x > 0) {
            animator.SetInteger("Direction", 3);
        } else if (velocity.x < 0) {
            animator.SetInteger("Direction", 2);
        }
    }

    public void RunAnim(Direction direction) {
        animator.SetBool("IsMoving", true);
        animator.SetInteger("Direction", (int)direction);
    }

    public void StopAnim() {
        animator.SetBool("IsMoving", false);
    } 
}
