using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttackAbility
{
    protected bool _canAttack = true;
    protected int _attackDamage = 0;
    protected float _coolDownTime = 0;
    abstract public void Attack(Entity ctx);
}
