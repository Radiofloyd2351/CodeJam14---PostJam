using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyStats : Entity
{


    const float ROTATION_VALUE_DEGREES = 2f;
    private int _attack;
    [SerializeField]
    private float speed;
    RaycastHit2D ray;
    private bool _isOver;
    private Target _target;
    private int time = 0;
    private bool found = false;
    private bool isRunningTimer = false;

    [SerializeField] private string enemyType;
    private AbsEnemyTargetting targetStrat;

    public void Start() {
        if (enemyType == "slime") {
            targetStrat = new ZoneEnemyTargetting();
            targetStrat.Execute(this);
        }
    }

}