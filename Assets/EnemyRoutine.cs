using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRoutine : MonoBehaviour
{
    const float ROTATION_VALUE_DEGREES = 2f;
    private int _attack;
    private int _health;
    [SerializeField]
    private float speed;
    RaycastHit2D ray;
    private Rigidbody2D _body;
    private Vector2 _direction = Vector2.left;
    private bool _isOver;
    private LayerMask mask;
    private Target _target;
    private int time = 0;
    private bool found = false;
    private bool isRunningTimer = false;

    public void Start()
    {
        mask = ~(1 << this.gameObject.layer);
        _body = GetComponent<Rigidbody2D>();

        StartCoroutine(FindPlayer());
    }


    private void Attack()
    {
        _target.VerifyTarget(this.gameObject);
    }

    public static Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    private void Move(Vector2 direction)
    {
        _body.velocity = direction;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _direction * 10);
    }

    IEnumerator FindPlayer()
    {


        while (time < 20)
        {
            if( time == 0) {
                found = false;
                StartCoroutine(Timer());
            }
        

            ray = Physics2D.Raycast(this.gameObject.transform.position, _direction, 10, mask);
            if (ray.collider != null) {
                _target = ray.collider.gameObject.GetComponent<Target>();
                if (_target != null)
                {
                    found = true;
                    time = 0;
                    while (isRunningTimer) { yield return null; }
                    Debug.Log("in");
                    
                    if ((ray.collider.gameObject.transform.position - this.gameObject.transform.position).magnitude < 2)
                    {
                        yield return AttackRoutine(_direction);
                    }
                    yield return GetToPlayer(_direction);
                }
                else
                {
                    _direction = Rotate(_direction, Mathf.Deg2Rad * ROTATION_VALUE_DEGREES);
                    yield return null;
                }
            }
            else
            {
                _direction = Rotate(_direction, Mathf.Deg2Rad * ROTATION_VALUE_DEGREES);
                yield return null;
            }
        }
        yield return GiveUp();
    }

    IEnumerator Timer()
    {
        isRunningTimer = true;
        while (time < 20 && !found)
        {
            time++;
            Debug.Log(time);
            yield return new WaitForSeconds(1f);
        }
        isRunningTimer = false;
    }

    IEnumerator AttackRoutine(Vector2 direction)
    {
        Attack();
        yield return new WaitForEndOfFrame();
    }

    IEnumerator GetToPlayer(Vector2 direction)
    {
        Move(direction);
        yield return new WaitForEndOfFrame();
        yield return FindPlayer();
    }

    IEnumerator GiveUp()
    {
        _body.velocity = Vector2.zero;
        yield return new WaitForFixedUpdate();
    }
}
