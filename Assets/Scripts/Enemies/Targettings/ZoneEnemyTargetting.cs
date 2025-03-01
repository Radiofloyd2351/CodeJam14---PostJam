using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEnemyTargetting : AbsEnemyTargetting {


    public override void Execute(EnemyStats ctx) {
        ctx.OnTriggerEnter += Target;
        ctx.OnTriggerExit += StopTargetting;
    }

    public IEnumerator Target(Entity target) {
        _ctx.movementStrat.target = target;
        yield return _ctx.movementStrat.FollowPlayer(_ctx);
        yield return null;
    }

    public IEnumerator StopTargetting(Entity target) {
        _ctx.movementStrat.target = null;
        yield return null;
    }
}
