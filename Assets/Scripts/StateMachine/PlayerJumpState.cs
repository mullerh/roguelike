using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        Debug.Log("JUMPING");
        if (playerMovement.cooldownUntilNextJump >= Time.time) {
            playerMovement.SwitchState(playerMovement.RunState);
            return;
        }
        playerMovement.jumpTimer = Time.time;

        Physics2D.IgnoreCollision(playerMovement.obstacles.GetComponent<Collider2D>(), playerMovement.GetComponent<Collider2D>());
    }

    public override void UpdateState(PlayerMovement playerMovement) {
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {
        if (playerMovement.jumpTimer + playerMovement.jumpTime > Time.time) {
                playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * 
                                                playerMovement.moveSpeed * Time.fixedDeltaTime * playerMovement.jumpSpeed);
        } else {
            playerMovement.cooldownUntilNextJump = Time.time + playerMovement.jumpCooldown;
            playerMovement.SwitchState(playerMovement.RunState);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {
        Physics2D.IgnoreCollision(playerMovement.obstacles.GetComponent<Collider2D>(), 
                                  playerMovement.GetComponent<Collider2D>(), false);
    }
}
