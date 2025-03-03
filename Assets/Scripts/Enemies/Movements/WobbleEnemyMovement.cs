using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEnemyMovement : AbsEnemyMovement 
{
    public float cycleloop = 0f;
    public float cooldown = 0f;
    public override IEnumerator Start(EnemyStats ctx) {

        isActive = true;
        while (true) {
            float angle = (Random.Range(0, 365));
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            ctx.SetDirection(new Vector2(x, y));
            ctx.gameObject.GetComponent<SpriteRenderer>().flipX = x > 0;
            ctx.animator.SetFloat("y", y);

            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(ctx.GetDirection() * ctx.speed);
            yield return new WaitForSeconds(cycleloop);
            ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            yield return new WaitForSeconds(cooldown);
        }
    }

    public override IEnumerator FollowPlayer(EnemyStats ctx) {
        isActive = false;
        ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        while (target != null) {
            ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            ctx.SetDirection((target.transform.position - ctx.gameObject.transform.position).normalized);
            ctx.gameObject.GetComponent<SpriteRenderer>().flipX = ctx.GetDirection().x > 0;
            ctx.animator.SetFloat("y", ctx.GetDirection().y);
            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(ctx.GetDirection() * ctx.speed * ctx.agressionModifier);
            yield return null;
        }

        yield return new WaitForSeconds(cycleloop);
        ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
       
        CoroutineManager.instance.RunCoroutine(Start(ctx), 10000 + ctx.id);
    }


    public override IEnumerator Stop(EnemyStats ctx) {
        yield return null;
    }
}
