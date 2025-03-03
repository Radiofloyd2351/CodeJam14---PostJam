using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : AbsPlayerMovementAbility
{
    const float SPEED_MULT = 0.4f;
    const int COOLDOWN_SECONDS = 15;
    const int TIMER_ID = 1500;
    int timer;
    bool isActive = false;
    bool isTimed = false;
    bool isAvailable = true;

    public SlowDown(bool isTimed = false,  int timer = 3) {
        _eventInstance = FMODUnity.RuntimeManager.CreateInstance(Audio.AudioDirectoryConstants.BASE_DIRECTORY_GAMEPLAY_ABILITIES + "Slow");
        this.isTimed = isTimed;
        this.timer = timer;
    }

    public override void Cancel(Entity ctx) {
        if (isActive) {
            ctx.speed /= SPEED_MULT;
            isActive = false;
            CoroutineManager.instance.RunCoroutine(StartCooldownTimer());
        }
        else 
        {
            CoroutineManager.instance.CancelCoroutine(TIMER_ID);
        }
        
    }

    public override void Move(Entity ctx) {
        if(!isAvailable) { return; }
        if(isActive) { return; }
        if(isTimed) {
            CoroutineManager.instance.RunCoroutine(ExcecuteTimer(ctx), TIMER_ID);
        }
        isActive = true;
        ctx.speed *= SPEED_MULT;
        Debug.Log("Slowed");
    }

    IEnumerator ExcecuteTimer(Entity ctx) {
        yield return new WaitForSeconds(timer);
        Cancel(ctx);
    }

    IEnumerator StartCooldownTimer() {
        isAvailable = false;
        yield return new WaitForSeconds(COOLDOWN_SECONDS);
        isAvailable = true;
    }



}
