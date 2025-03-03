using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void RunAnim(Vector2 velocity) {
        animator.SetBool("IsMoving", true);
        animator.SetFloat("x", velocity.x);
        animator.SetFloat("y", velocity.y);
    }

    public void RunAnim(Direction direction) {
        animator.SetBool("IsMoving", true);
        animator.SetInteger("Direction", (int)direction);
    }

    public void StopAnim() {
        animator.SetBool("IsMoving", false);
    } 
}
