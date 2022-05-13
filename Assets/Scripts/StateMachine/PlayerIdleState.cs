using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        Debug.Log("IDLE");
    }

    public override void UpdateState(PlayerMovement playerMovement) {
        if (playerMovement.shoot.ReadValue<Vector2>() != Vector2.zero) {
            playerMovement.SwitchState(playerMovement.ShootState);
        }
        if (playerMovement.move.triggered) {
            playerMovement.SwitchState(playerMovement.RunState);
        } else if (playerMovement.jump.triggered) {
            playerMovement.SwitchState(playerMovement.JumpState);
        }
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {}

    public override void ExitState(PlayerMovement playerMovement) {}
}
