using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDoor : Subscriber
{
    public Level levelName;
    public override void Run(GameState g, Level level) {
        if (level == levelName) {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
