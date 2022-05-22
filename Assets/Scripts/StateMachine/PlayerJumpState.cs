using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpTimer = 0;
    private RigidbodyConstraints rbConstraintsDefault = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    private Vector3 startDashPos;
    private Vector3 endDashPos;
    
    public override void EnterState(PlayerMovement playerMovement) {
        // debugging
        Debug.Log("JUMPING");
        playerMovement.rend.material.color = Color.blue;
        startDashPos = playerMovement.transform.position;

        // if cooldown is not over, go back to running
        if (playerMovement.cooldownUntilNextJump >= Time.time) {
            playerMovement.SwitchState(playerMovement.RunState);
            return;
        }

        // set timer and layer
        jumpTimer = Time.time;
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Jumper");
        playerMovement.rb.constraints = rbConstraintsDefault | RigidbodyConstraints.FreezePositionY;
    }

    public override void UpdateState(PlayerMovement playerMovement) {
    }

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {
        // Jump in moveDirection at movespeed * jumpspeed (otherwise switch back to run and set cooldown timer)
        if (jumpTimer + playerMovement.jumpTime > Time.time) {
            playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * 
                                           playerMovement.moveSpeed * Time.fixedDeltaTime * playerMovement.jumpSpeed);
        } else {
            playerMovement.cooldownUntilNextJump = Time.time + playerMovement.jumpCooldown;
            playerMovement.SwitchState(playerMovement.RunState);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {
        // reset layer to Player
        // if cooldown is not over, go back to running
        if (playerMovement.cooldownUntilNextJump >= Time.time) {
            playerMovement.SwitchState(playerMovement.RunState);
            return;
        }

        endDashPos = playerMovement.transform.position;
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Player");
        playerMovement.rb.constraints = rbConstraintsDefault;
        playerMovement.drawDashWall(startDashPos, endDashPos);
    }
}
