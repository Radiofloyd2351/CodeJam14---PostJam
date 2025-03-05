using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using Audio;

public class PlayerStats : Entity
{
    private TopDownCharacterController _controller;

    private void Start() {
        baseSoundDir = AudioDirectoryConstants.BASE_DIRECTORY_GAMEPLAY;
        // moveAbility = new Dash(15f, 3f, true);
        moveAbility = new ChainDash(15f, 10);
        // moveAbility = new SlowDown(true);
        // END TEST

        _controller = gameObject.GetComponent<TopDownCharacterController>();
    }

    public void PlayWalkSound() {
        PlaySound<int>(baseSoundDir + "Walk");
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
