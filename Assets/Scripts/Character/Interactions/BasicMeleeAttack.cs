using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAttack : AbsAttackAbility
{
    public BasicMeleeAttack(int attack = 2, float coolDown = 0.25f)
    {

        _attackDamage = attack;
        _coolDownTime = coolDown;
    }
    public override void Attack(Entity ctx)
    {
        if (_canAttack)
        {
            _canAttack = false;
            Debug.Log("Attacked for " + _attackDamage + " by: " + ctx.name);
            CoroutineManager.instance.StartCoroutine(WaitForAttackAvailable());
        }
    }

    private IEnumerator WaitForAttackAvailable()
    {
        yield return new WaitForSeconds(_coolDownTime);
        _canAttack = true;
    }
}
