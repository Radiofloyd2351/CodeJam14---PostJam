using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTarget : AbsEnemyState
{
    public override void CheckSwitchStates(EnemyStateMachine ctx) {
        if (ctx.canAttack) {
            SwitchStates(ctx, EnemyStateFactory.instance["Move"]);
        }
    }

    public override void EnterState(EnemyStateMachine ctx) {
        ctx.canAttack = false;
        ctx.StartAttackRoutine();
    }

    public override void ExitState(EnemyStateMachine ctx) {
    }

    public override void UpdateState(EnemyStateMachine ctx)
    {
    }
}
