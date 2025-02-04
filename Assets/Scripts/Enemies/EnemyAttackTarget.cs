using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTarget : AbsEnemyState
{
    public override void CheckSwitchStates(EnemyStateMachine ctx) {
        if (!ctx.VerifyInRange()) {
            SwitchStates(ctx, EnemyStateFactory.instance["Move"]);
        }
    }

    public override void EnterState(EnemyStateMachine ctx) {
        ctx.StartAttackRoutine();
    }

    public override void ExitState(EnemyStateMachine ctx)
    {
        ctx.StopAllCoroutines();
    }

    public override void UpdateState(EnemyStateMachine ctx)
    {
    }
}
