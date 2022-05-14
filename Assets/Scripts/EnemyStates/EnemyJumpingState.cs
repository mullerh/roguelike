using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpingState : EnemyBaseState
{
    private float jumpTimer = 0;

    public override void EnterState(EnemyBehaviour enemyBehaviour) {
        enemyBehaviour.enemySprite.color = Color.blue;
        jumpTimer = Time.time;

        Physics2D.IgnoreCollision(enemyBehaviour.obstacles.GetComponent<Collider2D>(), enemyBehaviour.GetComponent<Collider2D>());
    }

    public override void UpdateState(EnemyBehaviour enemyBehaviour) {}

    public override void FixedUpdateState(EnemyBehaviour enemyBehaviour) {
        if (jumpTimer + enemyBehaviour.jumpLength > Time.time) {
            enemyBehaviour.rb.MovePosition(enemyBehaviour.rb.position + enemyBehaviour.vectorToPlayer * 
                                           enemyBehaviour.moveSpeed * Time.fixedDeltaTime * enemyBehaviour.jumpSpeed);
        } else {
            enemyBehaviour.SwitchState(enemyBehaviour.AgroState);
        }
    }

    public override void IsShotState(EnemyBehaviour enemyBehaviour) {}

    public override void ExitState(EnemyBehaviour enemyBehaviour) {
            Physics2D.IgnoreCollision(enemyBehaviour.obstacles.GetComponent<Collider2D>(), 
                                      enemyBehaviour.GetComponent<Collider2D>(), false);
    }
}
