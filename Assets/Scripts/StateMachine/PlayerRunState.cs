using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        Debug.Log("RUNNING");
    }

    public override void UpdateState(PlayerMovement playerMovement) {
        if (playerMovement.shoot.ReadValue<Vector2>() != Vector2.zero) {
            playerMovement.SwitchState(playerMovement.ShootState);
        }

        if (playerMovement.jump.triggered) {
            playerMovement.SwitchState(playerMovement.JumpState);
        }
        
        if (playerMovement.move.ReadValue<Vector2>() == Vector2.zero) {
            playerMovement.SwitchState(playerMovement.IdleState);
        }
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {
        if (playerMovement.move.ReadValue<Vector2>() != Vector2.zero) {
            playerMovement.moveDirection = playerMovement.move.ReadValue<Vector2>();
            playerMovement.moveDirection.Normalize();

            playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * playerMovement.moveSpeed * Time.fixedDeltaTime);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {}

}
