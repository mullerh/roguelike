using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float timeElapsed = 0;
    private RigidbodyConstraints rbConstraintsDefault = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    private Vector3 startDashPos;
    private Vector3 endDashPos;

    private bool flag = false;
    private float tempDashTime;


    public override void EnterState(PlayerMovement playerMovement) {
        // debugging
        Debug.Log("JUMPING");
        playerMovement.rend.material.color = Color.blue;

        // if cooldown is not over, go back to running
        if (playerMovement.cooldownUntilNextJump >= Time.time) {
            playerMovement.SwitchState(playerMovement.RunState);
            return;
        }

        // check if there is an object in the way
        tempDashTime = playerMovement.jumpTime;
        RaycastHit hit;
        
        if (Physics.Linecast(playerMovement.transform.position, playerMovement.moveDirection * playerMovement.jumpSpeed * playerMovement.jumpTime, out hit)) {
            tempDashTime = hit.distance / playerMovement.jumpSpeed;
        }

        // set timer and layer and constraints
        flag = true;
        startDashPos = playerMovement.transform.position;
        timeElapsed = 0;
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Jumper");
        playerMovement.rb.constraints = rbConstraintsDefault | RigidbodyConstraints.FreezePositionY;
    }

    public override void UpdateState(PlayerMovement playerMovement) {}

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {
        // Jump in moveDirection at movespeed * jumpspeed (otherwise switch back to run and set cooldown timer)
        if (playerMovement.jumpTime > timeElapsed) {
            playerMovement.rb.MovePosition(playerMovement.rb.position + playerMovement.moveDirection * 
                                           Time.fixedDeltaTime * playerMovement.jumpSpeed);
            timeElapsed += Time.deltaTime;
        } else {
            playerMovement.cooldownUntilNextJump = Time.time + playerMovement.jumpCooldown;
            playerMovement.SwitchState(playerMovement.RunState);
        }
    }

    public override void ExitState(PlayerMovement playerMovement) {
        // if (playerMovement.cooldownUntilNextJump >= Time.time) {
        //     playerMovement.SwitchState(playerMovement.RunState);
        //     return;
        // }
        if (flag) {
            endDashPos = playerMovement.transform.position;
            playerMovement.drawDashWall(startDashPos, endDashPos);
            flag = false;
        }
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Player");
        playerMovement.rb.constraints = rbConstraintsDefault;
    }
}
