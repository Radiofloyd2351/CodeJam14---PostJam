using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindTarget : AbsEnemyState
{
    bool turnClock = true;
    bool debounce = false;
    public override void CheckSwitchStates(EnemyStateMachine ctx)
    {
        if (ctx.VerifyFoundTarget()) {
            SwitchStates(ctx, EnemyStateFactory.instance["Move"]);
        }
    }

    public override void EnterState(EnemyStateMachine ctx)
    {
        Debug.Log("Find Enter");
    }

    public override void ExitState(EnemyStateMachine ctx)
    {
        Debug.Log("Find Exit");    
    }

    public override void UpdateState(EnemyStateMachine ctx)
    {
        if(ctx.RotationValue <= 0 || (turnClock && ctx.RotationValue < 180))
        {
            turnClock = true;
            ctx.RotateClockwise();
        }
        else if (ctx.RotationValue >= 180 || (!turnClock && ctx.RotationValue > 0))
        {
            turnClock = false;
            ctx.RotateAntiClockwise();
        }
    }
}
