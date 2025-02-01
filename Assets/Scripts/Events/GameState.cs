using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField]
    Subscriber[] winners;
    [SerializeField]
    Subscriber[] starters;

    public void Awake() {
        Progression.gameState = this;  
    }
    public void Win(Level level) {
        /*
        if (winners != null) {
            foreach (Subscriber sub in winners) {
                sub.Run(this, level);
            }
        }*/
    }

    public void Open(Level level) {
        /*
        if (winners != null) {
            foreach (Subscriber sub in winners) {
                sub.Run(this, level);
            }
        }*/
    }

    public void Enter(Level level) {
        /*if (starters != null) {
            foreach (Subscriber sub in starters) {
                sub.Run(this, level);
            }
        }*/
    }
}
