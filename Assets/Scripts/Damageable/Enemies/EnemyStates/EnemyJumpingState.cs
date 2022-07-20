using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpingState : EnemyBaseState
{
    private float jumpTimer = 0;

    public override void EnterState(EnemyBehaviour enemyBehaviour) {
        //debugging
        enemyBehaviour.enemySprite.color = Color.blue;
        jumpTimer = Time.time;

        // change layer to EnemyJumper (for collision)
        enemyBehaviour.gameObject.layer = LayerMask.NameToLayer("EnemyJumper");
    }

    public override void UpdateState(EnemyBehaviour enemyBehaviour) {}

    public override void FixedUpdateState(EnemyBehaviour enemyBehaviour) {
        // Continue jumping while jumpLength not exceeded
        if (jumpTimer + enemyBehaviour.jumpLength > Time.time) {
            // Move in direction of player at jumpspeed * movespeed
            enemyBehaviour.rb.MovePosition(enemyBehaviour.rb.position + enemyBehaviour.vectorToPlayer * 
                                           enemyBehaviour.moveSpeed * Time.fixedDeltaTime * enemyBehaviour.jumpSpeed);
        } else {
            // break out of jump
            enemyBehaviour.SwitchState(enemyBehaviour.AgroState);
        }
    }

    public override void IsShotState(EnemyBehaviour enemyBehaviour) {}

    public override void ExitState(EnemyBehaviour enemyBehaviour) {
        // change layer to Enemies (for collision)
        enemyBehaviour.gameObject.layer = LayerMask.NameToLayer("Enemies");
    }
}
