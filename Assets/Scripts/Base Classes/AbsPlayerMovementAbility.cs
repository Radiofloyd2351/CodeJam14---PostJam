using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsPlayerMovementAbility : MonoBehaviour
{
    abstract public void Move(Rigidbody2D body, Vector2 direction);
    abstract public void Cancel(Rigidbody2D body);
}
