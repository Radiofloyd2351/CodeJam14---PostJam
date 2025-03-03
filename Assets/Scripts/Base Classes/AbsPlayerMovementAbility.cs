using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsPlayerMovementAbility
{
    abstract public void Move(Entity ctx);
    abstract public void Cancel(Entity ctx);
}
