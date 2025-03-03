using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbsEnemyState : IState<EnemyStateMachine>
{
    public abstract void CheckSwitchStates(EnemyStateMachine ctx);
    public abstract void EnterState(EnemyStateMachine ctx);
    public abstract void ExitState(EnemyStateMachine ctx);

    public void SwitchStates(EnemyStateMachine ctx, IState<EnemyStateMachine> state) {
        ctx.CurrState.ExitState(ctx); 
        state.EnterState(ctx);
        ctx.CurrState = state;
    }

    public abstract void UpdateState(EnemyStateMachine ctx);
    
}
