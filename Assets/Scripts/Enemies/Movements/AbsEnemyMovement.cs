using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsEnemyMovement {

    protected bool isActive = false;
    public Entity target;
    protected Rigidbody2D _body;
    public abstract IEnumerator Start(EnemyStats ctx);
    public abstract IEnumerator Stop(EnemyStats ctx);

    public abstract IEnumerator FollowPlayer(EnemyStats ctx);

    ~AbsEnemyMovement() { 
    }
}
