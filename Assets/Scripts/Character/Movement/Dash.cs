using System.Collections;
using UnityEngine;
using Audio;
using Unity.VisualScripting;

namespace Movement
{
    public class Dash : AbsPlayerMovementAbility
    {
        const float COOLDOWN = 0.750f;
        private float length = 3f;
        const int COROUTINE_DASH_ID = 2000;
        const int COROUTINE_COOLDOWN_ID = 3000;
        protected const int SLIDE_ID = 200;
        const float SLIDE_TIME_S = 0.1f;
        public float speed;
        private bool withSlide = false;
        protected bool _onCooldown = false;
        protected bool _isDashing = false;
        protected bool _isPenalised = false;

        public Dash(float speed = 15f, float length = 3f, bool withSlide = false)
        {
            this.speed = speed;
            this.length = length;
            this.withSlide = withSlide;
        }



        public override void Cancel(Entity ctx)
        {
            CoroutineManager.instance.RunCoroutine(CoolDown(), COROUTINE_COOLDOWN_ID);
        }

        public override void Move(Entity ctx)
        {
            if (ctx.PayStamina(1f)) {
                if (!_onCooldown && !_isDashing) {
                    CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_DASH_ID + ctx.id);
                } else if (_isDashing) {
                    _isPenalised = true;
                }
            }
        }

        protected IEnumerator Slide(Entity ctx, Vector2 entryVelocity, int amount = 3) {
            if (amount - 2 > 0) {
                float t = 0;
                ctx.Body.velocity = Vector3.zero;
                while (t < 1) {
                    ctx.Body.velocity = Vector2.Lerp(entryVelocity, Vector2.zero, t);
                    t += Time.fixedDeltaTime / (SLIDE_TIME_S * Mathf.Pow((amount - 2), 1.3f) * Mathf.Clamp(entryVelocity.magnitude / 10, 0, 1));

                    yield return null;
                }
            }
            ctx.StopDashAnim();
            ctx.EnableMovement();
        }

        protected IEnumerator CoolDown() 
        {
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown=false;
        }

        virtual protected IEnumerator DashFunction(Entity ctx)
        {
            ctx.RunDashAnim(ctx.GetLastDirection());
            CoroutineManager.instance.CancelCoroutine(SLIDE_ID + ctx.id);
            ctx.DisableMovement();
            ctx.Body.velocity = Vector3.zero;
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass * ctx.Body.drag);
            Debug.Log("SPEED IS: " + speed * 50 * ctx.GetLastDirection().normalized);
            _onCooldown = true;
            ctx.PlaySound<int>(ctx.baseSoundDir + "Abilities/Dash");
            yield return new WaitForSeconds(length/speed);
            ctx.PlaySound<int>(ctx.baseSoundDir + "Abilities/Land");
            Vector3 savedVelocity = ctx.Body.velocity;
            if (withSlide) {
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity, 10), SLIDE_ID + ctx.id);
            } else {
                ctx.StopDashAnim();
            }
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            ctx.EnableMovement();
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown = false;
        }
    }
}
