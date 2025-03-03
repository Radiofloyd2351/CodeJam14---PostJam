using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsEnemyTargetting
{

    protected EnemyStats _ctx;

    public void Init(EnemyStats ctx) {
        _ctx = ctx;
        Execute(ctx);
    }
    public abstract void Execute(EnemyStats ctx);
}
