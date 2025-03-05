using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Movement;


public class EnemyStats : Entity {


    const float ROTATION_VALUE_DEGREES = 2f;
    const string WALK_SOUND = "Walk";
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
            _walkSound = FMODUnity.RuntimeManager.CreateInstance(Audio.AudioDirectoryConstants.BASE_DIRECTORY_GAMEPLAY + WALK_SOUND);

            targetStrat = new ZoneEnemyTargetting();
            moveAbility = new Dash(10f, 3f, true);
            targetStrat.Init(this);
            movementStrat = new WobbleEnemyMovement();
            ((WobbleEnemyMovement)movementStrat).cooldown = 1f;
            ((WobbleEnemyMovement)movementStrat).cycleloop = 3f;
            CoroutineManager.instance.RunCoroutine(movementStrat.Start(this), 10000 + id);
        }
    }
}