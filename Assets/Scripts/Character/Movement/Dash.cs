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

        public Dash(float speed = 15f)
        {
            this.speed = speed;
        }



        public override void Cancel(Entity ctx)
        {
                //CoroutineManager.instance.CancelCoroutine(COROUTINE_ID + ctx.id);
                //ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        public override void Move(Entity ctx)
        {
            Debug.Log("AAA");
            if (!_onCooldown) {
                Debug.Log("BBB");
                CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_ID + ctx.id);
            }
        }

        virtual protected IEnumerator DashFunction(Entity ctx)
        {
            ctx.DisableControls();
            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(speed * 50 * ctx.GetLastDirection().normalized);
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            ctx.EnableControls();
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown = false;
        }
    }
}
