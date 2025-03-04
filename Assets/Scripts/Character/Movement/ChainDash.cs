using System.Collections;
using UnityEngine;


namespace Movement
{
    public class ChainDash : Dash {
        const float COOLDOWN = 1f;
        const float SLIDE_TIME_S = 0.1f;
        const float CONSISTENCY_LATENCY = 0.1f;
        const int SLIDE_ID = 200;
        const int TIME_WINDOW_ID = 300;
        const float LENGTH = 3f;
        private int _maxDashAmount;
        public int MaxDashAmount { set { _maxDashAmount = value; } }
        private int _currentDashAmount = 1;

        public ChainDash(Dash copy, int maxDashes = 2) {
            speed = copy.speed;
            _maxDashAmount = maxDashes;
        }

        public ChainDash(float speed = 15f, int maxDashes = 2) {
            this.speed = speed;
            _maxDashAmount = maxDashes;
        }

        public override void Cancel(Entity ctx) {
            if (_onCooldown) { return; }
            //CoroutineManager.instance.RunCoroutine(ResetDashes(ctx, ctx.Body.velocity), RESET_ID);
        }

        private IEnumerator TimeWindow(Entity ctx, float time) {
            _isDashing = false;
            ctx.EnableMovement();
            CoroutineManager.instance.tester.color = Color.blue;
            yield return new WaitForSeconds(time);
            CoroutineManager.instance.tester.color = Color.red;
            _onCooldown = true;
            yield return new WaitForSeconds(COOLDOWN);
            CoroutineManager.instance.tester.color = Color.green;
            _currentDashAmount = 1;
            _onCooldown = false;
        }

        private IEnumerator Slide(Entity ctx, Vector2 entryVelocity) {
            if ((_currentDashAmount - 2) > 0) {
                float t = 0;
                ctx.Body.velocity = Vector3.zero;
                while (t < 1) {
                    ctx.Body.velocity = Vector2.Lerp(entryVelocity, Vector2.zero, t);
                    t += Time.fixedDeltaTime / (SLIDE_TIME_S * Mathf.Pow((_currentDashAmount - 2), 1.3f) * Mathf.Clamp(entryVelocity.magnitude / 10, 0, 1));

                    yield return null;
                }
                ctx.RunAnim(ctx.GetLastDirection());
            }
        }

        override protected IEnumerator DashFunction(Entity ctx) {
            _isDashing = true;
            ctx.RunAnim(ctx.GetLastDirection());
            ctx.StopAnims();
            _currentDashAmount++;
            CoroutineManager.instance.CancelCoroutine(TIME_WINDOW_ID + ctx.id);
            CoroutineManager.instance.CancelCoroutine(SLIDE_ID + ctx.id);
            ctx.DisableMovement();
            Debug.Log("Dashed: " + _currentDashAmount);
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass);
            _eventInstance.start();
            yield return new WaitForSeconds(LENGTH/speed);
            Vector3 savedVelocity = ctx.Body.velocity;
            if (_currentDashAmount > _maxDashAmount) {
                _onCooldown = true;
                CoroutineManager.instance.RunCoroutine(TimeWindow(ctx, 0f), TIME_WINDOW_ID + ctx.id);
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity), SLIDE_ID + ctx.id);
            } else {
                CoroutineManager.instance.RunCoroutine(TimeWindow(ctx, CONSISTENCY_LATENCY * 1f), TIME_WINDOW_ID + ctx.id);
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity), SLIDE_ID + ctx.id);
                if (ctx.GetDirection().magnitude == 0) {
                    ctx.Body.velocity = Vector3.zero;
                }
            }
        }
    }
}
