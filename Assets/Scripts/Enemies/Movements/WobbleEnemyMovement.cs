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
        while (isActive) {
            float angle = (Random.Range(0, 365));
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            direction = new Vector2(x, y);


            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * ctx.speed);
            yield return new WaitForSeconds(cycleloop);
            ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            yield return new WaitForSeconds(cooldown);
        }
    }

    public override IEnumerator FollowPlayer(EnemyStats ctx) {
        Stop(ctx);
        yield return null;
        isActive = true;
        while(isActive) {
            if (target != null) {
                direction = (target.transform.position - ctx.gameObject.transform.position).normalized;
            } else {
                isActive = false;
            }

            ctx.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * ctx.speed);
            yield return new WaitForSeconds(cycleloop);
            Stop(ctx);
            yield return new WaitForSeconds(cooldown);
        }
    }

    public override IEnumerator Stop(EnemyStats ctx) {
        isActive = false;
        ctx.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return null;
    }
}
