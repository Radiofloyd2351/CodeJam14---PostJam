using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEnemyTargetting : AbsEnemyTargetting {


    public override void Execute(Entity ctx) {
        ctx.OnTrigger += Target;
    }

    public IEnumerator Target(Entity player) {
        Debug.Log(player.gameObject.name + " feur");
        yield return null;
    }
}
