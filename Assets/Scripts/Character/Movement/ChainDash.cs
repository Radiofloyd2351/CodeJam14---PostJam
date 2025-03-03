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

        private IEnumerator ResetDashes()
        {
            if (_currentDashAmount > 1)
            {
                yield return new WaitForSeconds(COOLDOWN);
                _currentDashAmount = 1;
            }
        }

        override protected IEnumerator DashFunction(Entity ctx)
        {
            Debug.Log("Dashed: " + _currentDashAmount);
            ctx.DisableControls();
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass);
            _eventInstance.start();
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            yield return new WaitForSeconds(CONSISTENCY_LATENCY);
            Debug.Log("finished dash");
            ctx.EnableControls();
            CoroutineManager.instance.RunCoroutine(ResetDashes(), ID);
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            Debug.Log("finished 2");
            _currentDashAmount++;
            if (_currentDashAmount > _maxDashAmount) {
                _currentDashAmount = 1;
                Debug.Log("finished 3");
                yield return new WaitForSeconds(COOLDOWN);
                Debug.Log("finished 4");
            }
            _onCooldown = false;
            Debug.Log("finished cooldown");
        }
    }
}
