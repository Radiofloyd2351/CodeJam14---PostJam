using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{
    private TopDownCharacterController _controller;

    private void Start() {

        _controller = gameObject.GetComponent<TopDownCharacterController>();
    }
    public override Vector2 GetDirection() {
        return _controller.Direction;
    }
    public override Vector2 GetLastDirection() {
        return _controller.LastDirection;
    }
    public override void SetLastDirection(Vector2 newDir) {
        _controller.LastDirection = newDir;
    }
    public override void EnableMovement() {
        _controller.EnableMovement();
    }
    public override void DisableMovement() {
        _controller.DisableMovement();
    }

}
