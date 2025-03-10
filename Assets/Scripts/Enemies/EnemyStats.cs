using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Movement;
using Audio;


public class EnemyStats : Entity {


    const float ROTATION_VALUE_DEGREES = 2f;
    private int _attack;
    [SerializeField]
    public float agressionModifier;
    RaycastHit2D ray;
    private bool _isOver;
    private Target _target;
    private int time = 0;
    private bool found = false;
    private bool isRunningTimer = false;
    public Vector2 direction;
    public Vector2 lastDirection;



    [SerializeField] public float aggressionDistance;
    [SerializeField] private string enemyType;
    private AbsEnemyTargetting targetStrat;
    public AbsEnemyMovement movementStrat;

    public override Vector2 GetDirection() {
        return direction;
    }
    public override void SetDirection(Vector2 newDir) {
        direction = newDir;
        if (newDir != Vector2.zero) {
            lastDirection = direction;
        }
    }

    public override Vector2 GetLastDirection() {
        return lastDirection;
    }
    public void Start() {
        if (enemyType == "slime") {
            baseSoundDir = AudioDirectoryConstants.BASE_DIRECTORY_SLIME;

            targetStrat = new ZoneEnemyTargetting();
            moveAbility = new Dash(15f, 5f, true);
            targetStrat.Init(this);
            movementStrat = new WobbleEnemyMovement();
            ((WobbleEnemyMovement)movementStrat).cooldown = 1f;
            ((WobbleEnemyMovement)movementStrat).cycleloop = 3f;
            Debug.Log(CoroutineManager.instance);
            CoroutineManager.instance.RunCoroutine(movementStrat.Start(this), 10000 + id);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<PlayerStats>() != null) {
            collision.gameObject.GetComponent<PlayerStats>().healthBar.PayValue(10);
        }
    }
}