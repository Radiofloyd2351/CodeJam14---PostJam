using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Dash : AbsPlayerMovementAbility
    {
        const int DURATION_MS = 100000;
        public float speed = 100;
        private bool isDashing = false;

        public override void Cancel(Rigidbody2D body)
        {
            if (isDashing) {
                isDashing = false;
                body.velocity = Vector3.zero;
            }
        }

        public override void Move(Rigidbody2D body, Vector2 direction)
        {
            if (!isDashing) {
                isDashing = true;
                StartCoroutine(DashFunction(body, direction));
            }
        }

        IEnumerator DashFunction(Rigidbody2D body, Vector2 direction)
        {
            body.velocity = (speed * direction);
            yield return new WaitForSeconds(DURATION_MS);
            Cancel(body);
        }
    }
}
