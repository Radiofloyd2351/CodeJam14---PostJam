using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyStats : Entity
{


    const float ROTATION_VALUE_DEGREES = 2f;
    private int _attack;
    [SerializeField]
    public float speed;
    RaycastHit2D ray;
    private bool _isOver;
    private Target _target;
    private int time = 0;
    private bool found = false;
    private bool isRunningTimer = false;

    [SerializeField] private string enemyType;
    private AbsEnemyTargetting targetStrat;
    public AbsEnemyMovement movementStrat;

    public void Start() {
        if (enemyType == "slime") {
            targetStrat = new ZoneEnemyTargetting();
            targetStrat.Init(this);
            movementStrat = new WobbleEnemyMovement();
            ((WobbleEnemyMovement)movementStrat).cooldown = 1f;
            ((WobbleEnemyMovement)movementStrat).cycleloop = 3f;
            StartCoroutine(movementStrat.Start(this));
        }
    }

}