using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


namespace Movement
{
    public class Dash : AbsPlayerMovementAbility
    {
        const int COOLDOWN = 1;
        const int LENGTH = 3;
        const int COROUTINE_ID = 20000;
        public float speed = 15;
        private bool onCooldown = false;
        private Vector2 lastSpeed = Vector2.zero;
        Coroutine currentRoutine = null;
        int i = 0;

        public override void Cancel(TopDownCharacterController ctx)
        {
            /*if (currentRoutine != null && isDashing) {
                CoroutineManager.instance.CancelCoroutine(COROUTINE_ID + ctx.id);
                isDashing = false;
                ctx.Body.velocity = Vector3.zero;
            }*/
        }

        public override void Move(TopDownCharacterController ctx)
        {
            if (!onCooldown)
            {
                CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_ID + ctx.id);
            }
        }

        IEnumerator DashFunction(TopDownCharacterController ctx)
        {
            i++;
            yield return new WaitForSeconds(0.01f);
            ctx.DisableControls();
            Debug.Log("Speed " + lastSpeed.normalized + "instances: " + i + " feur: " + ctx.Body.velocity);
            ctx.Body.velocity = speed * ctx.LastDirection.normalized;
            onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            ctx.EnableControls();
            i= 0;
            if (ctx.Direction.magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(COOLDOWN);
            onCooldown = false;
        }
    }
}
