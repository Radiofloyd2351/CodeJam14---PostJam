using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subscriber : MonoBehaviour
{
    public abstract void Run(GameState g, string level);
}