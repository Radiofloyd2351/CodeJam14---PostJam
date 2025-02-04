using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsStateMachine<Machine> : MonoBehaviour where Machine : AbsStateMachine<Machine>
{
    protected IState<Machine> _currState;
    public IState<Machine> CurrState { get { return _currState; } set { _currState = value; } }
}
