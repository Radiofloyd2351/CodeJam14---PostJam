using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LaunchColor {
    Red,
    Green,
    Blue,
    Null
}

public class LaunchPadButton : MonoBehaviour
{


    public void Start() {
        if (color == LaunchColor.Green) {
            manager.score++;
        }
    }

    public LaunchColor color;
    public LaunchpadGame manager;

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(manager.score);
        manager.ChangeColor(gameObject);
    }

}
