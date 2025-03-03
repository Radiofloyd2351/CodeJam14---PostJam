using System.Collections;
using UnityEngine;


namespace Movement
{
    public class Dash : AbsPlayerMovementAbility
    {
        const float COOLDOWN = 0.750f;
        const int LENGTH = 3;
        const int COROUTINE_ID = 20000;
        public float speed;
        protected bool _onCooldown = false;

        public Dash(float speed = 15)
        {
            this.speed = speed;
        }



        public override void Cancel(TopDownCharacterController ctx)
        {
                CoroutineManager.instance.CancelCoroutine(COROUTINE_ID + ctx.id);
                ctx.Body.velocity = Vector3.zero;
        }

        public override void Move(TopDownCharacterController ctx)
        {
            if (!_onCooldown)
            {
                CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_ID + ctx.id);
            }
        }

        virtual protected IEnumerator DashFunction(TopDownCharacterController ctx)
        {
            ctx.DisableControls();
            ctx.Body.AddForce(speed * ctx.LastDirection.normalized);
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            ctx.EnableControls();
            if (ctx.Direction.magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown = false;
        }
    }
}
