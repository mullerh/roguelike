using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpTimer = 0;
    
    public override void EnterState(PlayerMovement playerMovement) {
        Debug.Log("JUMPING");
        playerMovement.spriteRenderer.color = Color.blue;
        if (playerMovement.cooldownUntilNextJump >= Time.time) {
            playerMovement.SwitchState(playerMovement.RunState);
            return;
        }
        jumpTimer = Time.time;

        playerMovement.gameObject.layer = LayerMask.NameToLayer("Jumper");
    }

    public override void UpdateState(PlayerMovement playerMovement) {
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {
        if (jumpTimer + playerMovement.jumpTime > Time.time) {
            playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * 
                                           playerMovement.moveSpeed * Time.fixedDeltaTime * playerMovement.jumpSpeed);
        } else {
            playerMovement.cooldownUntilNextJump = Time.time + playerMovement.jumpCooldown;
            playerMovement.SwitchState(playerMovement.RunState);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
