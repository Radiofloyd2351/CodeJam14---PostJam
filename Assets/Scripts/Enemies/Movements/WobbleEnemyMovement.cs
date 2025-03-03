using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEnemyMovement : AbsEnemyMovement 
{
    public float cycleloop = 0f;
    public float cooldown = 0f;
    private SpriteRenderer _sprite;
    public override IEnumerator Start(EnemyStats ctx) {
        _body = ctx.gameObject.GetComponent<Rigidbody2D>();
        _sprite = ctx.gameObject.GetComponent<SpriteRenderer>();
        isActive = true;
        while (true) {
            float angle = (Random.Range(0, 365));
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            ctx.SetDirection(new Vector2(x, y));
            _sprite.flipX = x > 0;
            ctx.animator.SetFloat("y", y);

            _body.AddForce(ctx.GetDirection() * ctx.speed * _body.mass);
            yield return new WaitForSeconds(cycleloop);
            _body.velocity = Vector3.zero;
            yield return new WaitForSeconds(cooldown);
        }
    }

    public override IEnumerator FollowPlayer(EnemyStats ctx) {
        isActive = false;
        _body.velocity = Vector3.zero;
        while (target != null) {
            _body.velocity = Vector3.zero;
            ctx.SetDirection((target.transform.position - ctx.gameObject.transform.position).normalized);
            _sprite.flipX = ctx.GetDirection().x > 0;
            ctx.animator.SetFloat("y", ctx.GetDirection().y);
            _body.AddForce(ctx.GetDirection() * ctx.speed * ctx.agressionModifier * _body.mass);
            yield return null;
        }

        yield return new WaitForSeconds(cycleloop);
        _body.velocity = Vector3.zero;
       
        CoroutineManager.instance.RunCoroutine(Start(ctx), 10000 + ctx.id);
    }


    public override IEnumerator Stop(EnemyStats ctx) {
        yield return null;
    }
}
