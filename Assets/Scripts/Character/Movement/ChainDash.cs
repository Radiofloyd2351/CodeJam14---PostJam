using System.Collections;
using UnityEngine;


namespace Movement
{
    public class ChainDash : Dash
    {
        const float COOLDOWN = 0.750f;
        const float CONSISTENCY_LATENCY = 0.1f;
        const int ID = 100;
        const int LENGTH = 3;
        private int _maxDashAmount;
        public int MaxDashAmount { set {_maxDashAmount = value;} }
        private int _currentDashAmount = 1;

        private Coroutine resetCoroutine = null;

        public ChainDash(Dash copy, int maxDashes = 2)
        {
            speed = copy.speed;
            _maxDashAmount = maxDashes;
        }

        public ChainDash(float speed = 15, int maxDashes = 2)
        {
            this.speed = speed;
            _maxDashAmount = maxDashes;
        }

        private IEnumerator ResetDashes()
        {
            if (_currentDashAmount > 1)
            {
                yield return new WaitForSeconds(COOLDOWN);
                _currentDashAmount = 1;
            }
        }

        override protected IEnumerator DashFunction(TopDownCharacterController ctx)
        {
            Debug.Log("Dashed: " + _currentDashAmount);
            ctx.DisableControls();
            ctx.Body.AddForce(speed * 50 * ctx.LastDirection.normalized);
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            yield return new WaitForSeconds(CONSISTENCY_LATENCY);
            ctx.EnableControls();
            CoroutineManager.instance.RunCoroutine(ResetDashes(), ID);
            if (ctx.Direction.magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            _currentDashAmount++;
            if (_currentDashAmount > _maxDashAmount)
            {
                _currentDashAmount = 1;
                yield return new WaitForSeconds(COOLDOWN);
            }
                _onCooldown = false;
        }
    }
}
