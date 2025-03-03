using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : AbsStateMachine<EnemyStateMachine>
{
    private const float ROTATION_VALUE_DEGREES = 0.04f;  //aussi le speed wtf
    private const float ROTATION_SPEED = 50f;  //inversement prop
    private const float ROTATION_AMOUNT = 1/25f;
    private float _rotationValue = 0;
    public float RotationValue { get { return _rotationValue; } }
    private Target _target;
    RaycastHit2D ray;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotSpeed;
    public Target Target {  get { return _target; } }
    //private Vector2 _direction = Vector2.left;
    private Rigidbody2D _body;
    private LayerMask _mask;

    private bool _foundTarget;
    private bool _hitTarget;

    public bool canAttack = true;
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
        /*ray = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.rotation * _direction, 10, _mask);
        if (ray.collider != null)
        {
            _target = ray.collider.gameObject.GetComponent<Target>();
            if (_target != null)
            {
                return (ray.collider.gameObject.transform.position - this.gameObject.transform.position).magnitude < 2;
            }
        }
        return false;*/
        return _hitTarget;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Target>() != null && _currState == EnemyStateFactory.instance["Move"]) {
            _target = collision.gameObject.GetComponent<Target>();
            _hitTarget = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Target>() != null && _currState == EnemyStateFactory.instance["Move"]) {
            _target = null;
            _hitTarget = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Target>() != null) {
            _target = collision.GetComponent<Target>();
            _foundTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Target>() != null) {
            _target = collision.GetComponent<Target>();
            Vector3 distance = Quaternion.Euler(-transform.rotation.eulerAngles) * (_target.transform.position - transform.position);
            if (distance.y > 0) {
                _rotationValue = 0;
            } else {
                _rotationValue = 180;
            }
            _target = null;
            _foundTarget = false;
        }
    }

    public bool VerifyFoundTarget() {
        /*_target = null;
        ray = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.rotation *  _direction, 10, _mask);
        if (ray.collider != null)
        {
            _target = ray.collider.gameObject.GetComponent<Target>();
        }
        return _target != null;*/
        return _foundTarget;
    }

    void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, this.gameObject.transform.rotation * _direction * 10);*/
    }

    public void Move()
    {
        Debug.Log(_target);
        if (_target != null) {
            Vector3 distance = Quaternion.Euler(-transform.rotation.eulerAngles) * (_target.transform.position - transform.position);
            Debug.Log(distance);
            if (distance.y > 0.2) {
                RotateClockwise();
                StartCoroutine(MoveRoutine());
            } else if (distance.y < -0.2) {
                RotateAntiClockwise();
                StartCoroutine(MoveRoutine());
            } else {
                _body.velocity = (this.gameObject.transform.rotation * new Vector3(1f, 0f, 0f) * moveSpeed);
                StartCoroutine(DisableTarget());
            }
        }
    }

    private IEnumerator DisableTarget() {
        while (_target != null) {
            if (Vector3.Distance(transform.position, _target.transform.position) < 1f) {
                yield return _target.HitThis(1f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator MoveRoutine() {
        yield return new WaitForSeconds(0.01f);
        Move();
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
        //_direction = Rotate(_direction, ROTATION_VALUE_DEGREES);
        transform.eulerAngles +=  new Vector3(0f, 0f, ROTATION_VALUE_DEGREES * ROTATION_SPEED * rotSpeed);
        _rotationValue += ROTATION_VALUE_DEGREES / ROTATION_AMOUNT;
    }

    public void RotateAntiClockwise()
    {
        //_direction = Rotate(_direction, -ROTATION_VALUE_DEGREES);
        transform.eulerAngles -= new Vector3(0f, 0f, ROTATION_VALUE_DEGREES * ROTATION_SPEED * rotSpeed);
        _rotationValue -= ROTATION_VALUE_DEGREES / ROTATION_AMOUNT;
    }

    public IEnumerator AttackRoutine() {
        if (_target != null && _target.VerifyTarget(this.gameObject)) {
            //StartCoroutine(_target.HitThis(2f));
            Debug.Log("BOOM DAMAGE");
        }
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

}
