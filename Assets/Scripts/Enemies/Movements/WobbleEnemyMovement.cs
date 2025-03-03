using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEnemyMovement : AbsEnemyMovement 
{
    public float cycleloop = 0f;
    public float cooldown = 0f;
    private Vector2 direction;
    public override IEnumerator Start(EnemyStats ctx) {

        isActive = true;
        while (true) {
            float angle = (Random.Range(0, 365));
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            direction = new Vector2(x, y);
            ctx.gameObject.GetComponent<SpriteRenderer>().flipX = x > 0;
            ctx.animator.SetFloat("y", y);

            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * ctx.speed);
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
            direction = (target.transform.position - ctx.gameObject.transform.position).normalized;
            ctx.gameObject.GetComponent<SpriteRenderer>().flipX = direction.x > 0;
            ctx.animator.SetFloat("y", direction.y);
            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * ctx.speed * ctx.agressionModifier);
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
