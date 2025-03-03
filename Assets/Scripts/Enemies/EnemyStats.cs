using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyStats : Entity
{


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
    [SerializeField] public Animator animator;
    public Vector2 direction;

    [SerializeField] private string enemyType;
    private AbsEnemyTargetting targetStrat;
    public AbsEnemyMovement movementStrat;

    public override Vector2 GetDirection() {
        return direction;
    }
    public override void SetDirection(Vector2 newDir) {
        direction = newDir;
    }

    public void Start() {
        if (enemyType == "slime") {
            targetStrat = new ZoneEnemyTargetting();
            targetStrat.Init(this);
            movementStrat = new WobbleEnemyMovement();
            ((WobbleEnemyMovement)movementStrat).cooldown = 1f;
            ((WobbleEnemyMovement)movementStrat).cycleloop = 3f;
            CoroutineManager.instance.RunCoroutine(movementStrat.Start(this), 10000 + id);
        }
    }
}