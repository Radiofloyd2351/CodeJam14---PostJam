using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Level {
    Blank = 0,
    Tech = 1,
    Nature = 2,
    Hell = 3
}
public static class Progression
{

    public static Level lastUnlock;
    public static GameState gameState;

    public static Dictionary<Level, bool> unlocks = new() {
        { Level.Blank, false },
        { Level.Tech, false },
        { Level.Nature, false },
        { Level.Hell, false }
    };

    public static void Win(Level level) {
        gameState.Win(level);
        unlocks[level] = true;
        lastUnlock = level;
    }

    public static void Open(Level level) {
        gameState.Open(level);
    }


    public static void Enter(Level level) {
        gameState.Enter(level);
        unlocks[level] = true;
        lastUnlock = level;
    }
}
