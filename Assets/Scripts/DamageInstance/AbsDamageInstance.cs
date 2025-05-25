using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsDamageInstance
{
    protected int inDamage = 0;
    protected int outDamage = 0;
    protected int tickDamage = 0;
    protected int lifeTime = 1;
    public abstract void Damage(bool isTick = false, bool isOut = false);
}
