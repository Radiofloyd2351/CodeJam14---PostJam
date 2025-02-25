using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory: MonoBehaviour {
    private Dictionary<string, AbsEnemyState> _states = new();
    public static EnemyStateFactory instance;

    private void Start() {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IState<EnemyStateMachine> GetState(string name)
    {
        switch (name)
        {
            case "Find":
                if (!_states.ContainsKey(name))
                {
                    _states.Add(name, new EnemyFindTarget());
                }
                break;
            case "Attack":
                if (!_states.ContainsKey(name))
                {
                    _states.Add(name, new EnemyAttackTarget());
                }
                break;
            case "Lose":
                if (!_states.ContainsKey(name))
                {
                    _states.Add(name, new EnemyLoseTarget());
                }
                break;
            case "Move":
                if (!_states.ContainsKey(name))
                {
                    _states.Add(name, new EnemyMoveToTarget());
                }
                break;
        }
        return _states[name];
    }

    public IState<EnemyStateMachine> this[string name] => GetState(name);
}
