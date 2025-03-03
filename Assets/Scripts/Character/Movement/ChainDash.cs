using System.Collections;
using UnityEngine;


namespace Movement
{
    public class ChainDash : Dash
    {
        const float COOLDOWN = 3f;
        const float SLIDE_TIME_S = 0.750f;
        const float CONSISTENCY_LATENCY = 0.1f;
        const int RESET_ID = 100;
        const int LENGTH = 3;
        private int _maxDashAmount;
        public int MaxDashAmount { set {_maxDashAmount = value;} }
        private int _currentDashAmount = 1;

        public ChainDash(Dash copy, int maxDashes = 2)
        {
            speed = copy.speed;
            _maxDashAmount = maxDashes;
        }

        public ChainDash(float speed = 15f, int maxDashes = 2)
        {
            this.speed = speed;
            _maxDashAmount = maxDashes;
        }

        public override void Cancel(Entity ctx) {
            if (_onCooldown) {  return; }
            CoroutineManager.instance.RunCoroutine(ResetDashes(ctx, ctx.Body.velocity), RESET_ID);
        }

        private IEnumerator ResetDashes(Entity ctx, Vector2 entryVelocity)
        {
            if (_currentDashAmount > 1)
            {
                yield return new WaitForSeconds(3.5f * CONSISTENCY_LATENCY);
                _onCooldown = true;
                if (_currentDashAmount > 3) 
                {
                    yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, entryVelocity));
                }
                yield return new WaitForSeconds(COOLDOWN);
                _currentDashAmount = 1;
                _onCooldown = false;
            }
        }

        private IEnumerator Slide(Entity ctx, Vector2 entryVelocity) {
            float t = 0;
            while (t < 1) {
                ctx.Body.velocity = Vector2.Lerp(entryVelocity, Vector2.zero, t);
                t += Time.fixedDeltaTime/(SLIDE_TIME_S * Mathf.Clamp(entryVelocity.magnitude/10, 0, 1));
                yield return null;
            }
        }

        override protected IEnumerator DashFunction(Entity ctx)
        {
            Debug.Log("Dashed: " + _currentDashAmount);
            ctx.DisableControls();
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass);
            _eventInstance.start();
            yield return new WaitForSeconds(LENGTH/speed);
            yield return new WaitForSeconds(CONSISTENCY_LATENCY);
            Vector3 savedVelocity = ctx.Body.velocity;
            CoroutineManager.instance.RunCoroutine(ResetDashes(ctx, savedVelocity), RESET_ID);
            ctx.EnableControls();
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            _currentDashAmount++;
            if (_currentDashAmount > _maxDashAmount) {
                ctx.DisableControls();
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity));
                ctx.EnableControls();
                yield return new WaitForSeconds(COOLDOWN);
                _currentDashAmount = 1;
                _onCooldown = false;
            }
        }
    }
}
