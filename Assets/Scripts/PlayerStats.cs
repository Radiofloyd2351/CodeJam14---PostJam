using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{


    private TopDownCharacterController controller;

    private void Start() {
        controller = gameObject.GetComponent<TopDownCharacterController>();
    }
    public override Vector2 GetDirection() {
        return controller.Direction;
    }
    public override Vector2 GetLastDirection() {
        return controller.LastDirection;
    }
    public override void SetLastDirection(Vector2 newDir) {
        controller.LastDirection = newDir;
    }
    public override void EnableControls() {
        controller.EnableControls();
    }
    public override void DisableControls() {
        controller.DisableControls();
    }

}
