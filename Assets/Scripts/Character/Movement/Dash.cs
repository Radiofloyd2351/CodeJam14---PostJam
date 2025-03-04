using System.Collections;
using UnityEngine;
using Audio;

namespace Movement
{
    public class Dash : AbsPlayerMovementAbility
    {
        const float COOLDOWN = 0.750f;
        const float LENGTH = 3f;
        const int COROUTINE_DASH_ID = 2000;
        const int COROUTINE_COOLDOWN_ID = 3000;
        public float speed;
        protected bool _onCooldown = false;
        protected bool _isDashing = false;

        public Dash(float speed = 15f)
        {
            _eventInstance = FMODUnity.RuntimeManager.CreateInstance(AudioDirectoryConstants.BASE_DIRECTORY_GAMEPLAY_ABILITIES + "Dash");
            this.speed = speed;
        }



        public override void Cancel(Entity ctx)
        {
            CoroutineManager.instance.RunCoroutine(CoolDown(), COROUTINE_COOLDOWN_ID);
        }

        public override void Move(Entity ctx)
        {
            if (!_onCooldown && !_isDashing) {
                CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_DASH_ID + ctx.id);
            }
        }

        protected IEnumerator CoolDown() 
        {
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown=false;
        }

        virtual protected IEnumerator DashFunction(Entity ctx)
        {
            ctx.RunAnim(ctx.GetLastDirection());
            ctx.StopAnims();
            ctx.DisableMovement();
            ctx.Body.velocity = Vector3.zero;
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass * ctx.Body.drag);
            _eventInstance.start();
            Debug.Log("SPEED IS: " + speed * 50 * ctx.GetLastDirection().normalized);
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            ctx.EnableMovement();
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
        }
    }
}
