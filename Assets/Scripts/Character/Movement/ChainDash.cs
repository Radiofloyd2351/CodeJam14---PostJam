using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    public class ChainDash : Dash {
        const float COOLDOWN = 1f;
        const float TIME_WINDOW_MS = 0.4f;
        const float PENALITY_WINDOW_MS = 0.2f;
        const int TIME_WINDOW_ID = 300;
        const float TIME_WINDOW_S = 1f;
        const float LENGTH = 2f;
        private int _maxDashAmount;
        private int _currentDashAmount;
        public int MaxDashAmount { set { _maxDashAmount = value; } }

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
        }

        private IEnumerator TimeWindow(Entity ctx, float time) {
            CoroutineManager.instance.tester.color = Color.cyan;
            yield return new WaitForSeconds(PENALITY_WINDOW_MS);
            _isDashing = false;
            ctx.EnableMovement();
            CoroutineManager.instance.tester.color = Color.blue;
            yield return new WaitForSeconds(time);
            CoroutineManager.instance.tester.color = Color.red;
            _onCooldown = true;
            yield return new WaitForSeconds(COOLDOWN);
            _currentDashAmount = 1;
            CoroutineManager.instance.tester.color = Color.green;
            _onCooldown = false;
            _isPenalised = false;
        }



        override protected IEnumerator DashFunction(Entity ctx) {
            _isDashing = true;
            ctx.RunDashAnim(ctx.GetLastDirection());
            _currentDashAmount++;
            CoroutineManager.instance.CancelCoroutine(TIME_WINDOW_ID + ctx.id);
            CoroutineManager.instance.CancelCoroutine(SLIDE_ID + ctx.id);
            ctx.DisableMovement();
            Debug.Log("Dashed: " + _currentDashAmount + " and is penalised:  " + _isPenalised);
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass);
            ctx.PlaySound<int>(ctx.baseSoundDir + "Abilities/Dash", new KeyValuePair<string, int>("ContinuousDashes", Mathf.Clamp(_currentDashAmount - 1, 0, 10)));
            yield return new WaitForSeconds(LENGTH/speed);
            Vector3 savedVelocity = ctx.Body.velocity;
            if (_currentDashAmount > _maxDashAmount || _isPenalised) {
                if (_isPenalised) {
                    ctx.staminaBar.PayValue(ctx.staminaBar.GetValue());
                }
                _onCooldown = true;
                CoroutineManager.instance.RunCoroutine(TimeWindow(ctx, 0f), TIME_WINDOW_ID + ctx.id);
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity, _currentDashAmount), SLIDE_ID + ctx.id);
            } else {
                CoroutineManager.instance.RunCoroutine(TimeWindow(ctx, TIME_WINDOW_MS), TIME_WINDOW_ID + ctx.id);
                
                yield return CoroutineManager.instance.RunCoroutine(Slide(ctx, savedVelocity, _currentDashAmount), SLIDE_ID + ctx.id);
                
                if (ctx.GetDirection().magnitude == 0) {
                    ctx.Body.velocity = Vector3.zero;
                }
            }
        }
    }
}
