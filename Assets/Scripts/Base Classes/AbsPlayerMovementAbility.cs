using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsPlayerMovementAbility
{
    abstract public void Move(TopDownCharacterController ctx);
    abstract public void Cancel(TopDownCharacterController ctx);
}
