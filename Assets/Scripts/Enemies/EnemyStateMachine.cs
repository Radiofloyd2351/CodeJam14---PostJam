using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : AbsStateMachine<EnemyStateMachine>
{
    private const float ROTATION_VALUE_DEGREES = 0.01f;
    private const float ROTATION_MULTIPLIER = 100f;
    private float _rotationValue = 0;
    public float RotationValue { get { return _rotationValue; } }
    private Target _target;
    RaycastHit2D ray;
    [SerializeField]
    private float speed;
    public Target Target {  get { return _target; } }
    private Vector2 _direction = Vector2.left;
    private Rigidbody2D _body;
    private LayerMask _mask;
    public void Start()
    {
        _mask = ~(1 << this.gameObject.layer);
        _body = GetComponent<Rigidbody2D>();
        _currState = EnemyStateFactory.instance["Find"];
        Debug.Log(_currState);
        _currState.EnterState(this);
    }

    public void Update()
    {
        _currState.CheckSwitchStates(this);
        _currState.UpdateState(this);
    }


    public bool VerifyInRange()
    {
        ray = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.rotation * _direction, 10, _mask);
        if (ray.collider != null)
        {
            _target = ray.collider.gameObject.GetComponent<Target>();
            if (_target != null)
            {
                return (ray.collider.gameObject.transform.position - this.gameObject.transform.position).magnitude < 2;
            }
        }
        return false;
    }
        

    public bool VerifyFoundTarget() {
        _target = null;
        ray = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.rotation *  _direction, 10, _mask);
        if (ray.collider != null)
        {
            _target = ray.collider.gameObject.GetComponent<Target>();
        }
        return _target != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, this.gameObject.transform.rotation * _direction * 10);
    }

    public void Move()
    {
        _body.velocity = (this.gameObject.transform.rotation *_direction * speed);
    }

    public void Stop()
    {
        _body.velocity = Vector2.zero;
    }

    public Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public void StartAttackRoutine() { 
        StartCoroutine(AttackRoutine());
    }

    public void RotateClockwise() {
        _direction = Rotate(_direction, ROTATION_VALUE_DEGREES);
        transform.eulerAngles +=  new Vector3(0f, 0f, ROTATION_VALUE_DEGREES * ROTATION_MULTIPLIER);
        _rotationValue += ROTATION_VALUE_DEGREES * ROTATION_MULTIPLIER;
    }

    public void RotateAntiClockwise()
    {
        _direction = Rotate(_direction, -ROTATION_VALUE_DEGREES);
        transform.eulerAngles -= new Vector3(0f, 0f, ROTATION_VALUE_DEGREES * ROTATION_MULTIPLIER);
        _rotationValue -= ROTATION_VALUE_DEGREES * ROTATION_MULTIPLIER;
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_target.VerifyTarget(this.gameObject))
            {
                Debug.Log("BOOM DAMAGE");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
