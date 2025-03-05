using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEnemyMovement : AbsEnemyMovement 
{
    public float cycleloop = 0f;
    public float cooldown = 0f;
    public float walkSoundCycle = 1f;
    const int START_ROUTINE_ID = 10000;
    private SpriteRenderer _sprite;
    public override IEnumerator Start(EnemyStats ctx) {
        _body = ctx.gameObject.GetComponent<Rigidbody2D>();
        _sprite = ctx.gameObject.GetComponent<SpriteRenderer>();
        isActive = true;
        while (true) {
            float angle = (Random.Range(0, 365));
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            Vector2 dir = new Vector2(x, y);
            ctx.SetDirection(dir);
            _sprite.flipX = x > 0;
            ctx.RunMoveAnim(dir);

            ctx.PlaySound<int>(ctx.baseSoundDir + "Walk");

            _body.AddForce(ctx.GetDirection() * ctx.speed * _body.mass);
            yield return new WaitForSeconds(cycleloop);
            _body.velocity = Vector3.zero;
            yield return new WaitForSeconds(cooldown);
        }
    }

    public override IEnumerator FollowTarget(EnemyStats ctx) {
        isActive = false;
        _body.velocity = Vector3.zero;
        float i = 0;
        while (targetting) {

            ctx.PlaySound<int>(ctx.baseSoundDir + "Walk");

            if (i >= 1) {
                i = 0;
            }
            i += Time.deltaTime;

            _body.velocity = Vector3.zero;
            ctx.SetDirection((target.transform.position - ctx.gameObject.transform.position).normalized);
            _sprite.flipX = ctx.GetDirection().x > 0;
            ctx.RunMoveAnim(ctx.GetDirection());
            _body.AddForce(ctx.GetDirection() * ctx.speed * ctx.agressionModifier * _body.mass);
            if (Vector3.Distance(target.gameObject.transform.position, ctx.gameObject.transform.position) < ctx.aggressionDistance) {
                CoroutineManager.instance.tester.color = Color.blue;
                ctx.SetDirection(Vector2.zero);
                _body.velocity = Vector2.zero;
                ctx.PlaySound<int>(ctx.baseSoundDir + "WindUp");
                yield return new WaitForSeconds(1f);
                ctx.SetDirection((target.transform.position - ctx.gameObject.transform.position).normalized);
                ctx.RunMoveAnim(ctx.GetDirection());
                CoroutineManager.instance.tester.color = Color.red;
                ctx.moveAbility.Move(ctx);
                yield return new WaitForSeconds(1f);
                CoroutineManager.instance.tester.color = Color.green;
            }
            yield return null;
        }

        yield return new WaitForSeconds(cycleloop);
        _body.velocity = Vector3.zero;
       
        CoroutineManager.instance.RunCoroutine(Start(ctx), START_ROUTINE_ID + ctx.id);
    }
    public override IEnumerator Stop(EnemyStats ctx) {
        yield return null;
    }
}
