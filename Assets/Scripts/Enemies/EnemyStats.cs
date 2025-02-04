using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    const float ROTATION_VALUE_DEGREES = 2f;
    private int _attack;
    private int _health;
    [SerializeField]
    private float speed;
    RaycastHit2D ray;
    private bool _isOver;
    private Target _target;
    private int time = 0;
    private bool found = false;
    private bool isRunningTimer = false;

}