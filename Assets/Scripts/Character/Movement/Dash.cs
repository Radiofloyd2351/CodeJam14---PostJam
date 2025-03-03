using System.Collections;
using UnityEngine;
using Audio;

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
            _eventInstance = FMODUnity.RuntimeManager.CreateInstance(AudioDirectoryConstants.BASE_DIRECTORY_GAMEPLAY + "AbDash");
            this.speed = speed;
        }



        public override void Cancel(Entity ctx)
        {

        }

        public override void Move(Entity ctx)
        {
            if (!_onCooldown) {
                CoroutineManager.instance.RunCoroutine(DashFunction(ctx), COROUTINE_ID + ctx.id);
            }
        }

        virtual protected IEnumerator DashFunction(Entity ctx)
        {
            ctx.DisableControls();
            ctx.Body.AddForce(speed * 50 * ctx.GetLastDirection().normalized * ctx.Body.mass);
            _eventInstance.start();
            Debug.Log("SPEED IS: " + speed * 50 * ctx.GetLastDirection().normalized);
            _onCooldown = true;
            yield return new WaitForSeconds(LENGTH/speed);
            ctx.EnableControls();
            if (ctx.GetDirection().magnitude == 0)
            {
                ctx.Body.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(COOLDOWN);
            _onCooldown = false;
        }
    }
}
