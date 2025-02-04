using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<Machine> where Machine: AbsStateMachine<Machine>
{
    public abstract void EnterState(Machine ctx);
    public abstract void UpdateState(Machine ctx);
    public abstract void CheckSwitchStates(Machine ctx);
    public abstract void ExitState(Machine ctx);
    public abstract void SwitchStates(Machine ctx, IState<Machine> state);

}
