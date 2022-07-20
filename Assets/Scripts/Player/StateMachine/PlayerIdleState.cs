using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    // RigidbodyConstraints originalConstraints;
    public override void EnterState(PlayerMovement playerMovement) {
        // debugging
        playerMovement.rend.material.color = Color.white;
        Debug.Log("IDLE");
        // originalConstraints = playerMovement.rb.constraints;
        // playerMovement.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateState(PlayerMovement playerMovement) {
        // switch to shoot if fire input is read
        if (playerMovement.shoot.ReadValue<Vector2>() != Vector2.zero) {
            playerMovement.SwitchState(playerMovement.ShootState);
        }
        // switch to run/jump if correct input is read
        if (playerMovement.move.triggered) {
            playerMovement.SwitchState(playerMovement.RunState);
        } else if (playerMovement.jump.triggered) {
            playerMovement.SwitchState(playerMovement.JumpState);
        }
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {}

    public override void ExitState(PlayerMovement playerMovement) {
        // playerMovement.rb.constraints = originalConstraints;
    }
}
