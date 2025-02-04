using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveToTarget : AbsEnemyState
{
    public override void CheckSwitchStates(EnemyStateMachine ctx)
    {
        if (ctx.VerifyInRange()) {
            SwitchStates(ctx, EnemyStateFactory.instance["Attack"]);
        }
        if (!ctx.VerifyFoundTarget())
        {
            SwitchStates(ctx, EnemyStateFactory.instance["Find"]);
        }
    }

    public override void EnterState(EnemyStateMachine ctx)
    {
        ctx.Move();
    }

    public override void ExitState(EnemyStateMachine ctx)
    {
        ctx.Stop();
    }

    public override void UpdateState(EnemyStateMachine ctx)
    {
    }
}
