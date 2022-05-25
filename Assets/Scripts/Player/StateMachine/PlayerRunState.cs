using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        // debugging
        playerMovement.rend.material.color = Color.yellow;
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
        // Move in direction of move input at movespeed (check for zero is to remember last direction moved, when leaving run state)
        Vector2 tempDir = playerMovement.move.ReadValue<Vector2>();
        Vector3 moveVec = new Vector3(tempDir.x, 0, tempDir.y);
        if (moveVec != Vector3.zero) {
            moveVec.Normalize();

            float targetAngle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + playerMovement.cam.eulerAngles.y;
            playerMovement.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            playerMovement.moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerMovement.moveDirection.Normalize();

            playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * playerMovement.moveSpeed * Time.fixedDeltaTime);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {}

}
