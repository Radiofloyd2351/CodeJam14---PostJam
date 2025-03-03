using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : AbsPlayerMovementAbility
{
    const float SPEED_MULT = 0.4f;
    const int COOLDOWN_SECONDS = 10;
    private int _remainingStamina;
    const int TIMER_ID = 1500;
    int timer;
    bool isActive = false;
    bool isTimed = false;
    bool isAvailable = true;

    public SlowDown(bool isTimed = false,  int timer = 3) {
        this.isTimed = isTimed;
        this.timer = timer;
    }

    public override void Cancel(TopDownCharacterController ctx) {
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

    public override void Move(TopDownCharacterController ctx) {
        if(!isAvailable) { return; }
        if(isActive) { return; }
        if(isTimed) {
            _remainingStamina = timer;
            CoroutineManager.instance.RunCoroutine(ExcecuteTimer(ctx), TIMER_ID);
        }
        isActive = true;
        ctx.speed *= SPEED_MULT;
        Debug.Log("Slowed");
    }

    IEnumerator ExcecuteTimer(TopDownCharacterController ctx) {
        Debug.Log("Timed: " + _remainingStamina);
        yield return WaitForSecondsAndSaveTime(ctx);
    }

    IEnumerator WaitForSecondsAndSaveTime(TopDownCharacterController ctx) {
        while (isActive && _remainingStamina > 0) {
            yield return new WaitForSeconds(1);
            Debug.Log(_remainingStamina);
            _remainingStamina--;
        }
        if(_remainingStamina <= 0) {
            Cancel(ctx);
        }
    }

    IEnumerator StartCooldownTimer() {
        isAvailable = false;
        yield return new WaitForSeconds(COOLDOWN_SECONDS);
        isAvailable = true;
    }



}
