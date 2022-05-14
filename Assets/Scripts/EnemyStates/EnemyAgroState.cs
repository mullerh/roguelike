using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroState : EnemyBaseState
{
    public override void EnterState(EnemyBehaviour enemyBehaviour) {
        enemyBehaviour.enemySprite.color = Color.red;
    }

    public override void UpdateState(EnemyBehaviour enemyBehaviour) {
        if (enemyBehaviour.distanceToPlayer > enemyBehaviour.stopAgroThreshold) {
            enemyBehaviour.SwitchState(enemyBehaviour.IdleState);
        }
    }

    public override void FixedUpdateState(EnemyBehaviour enemyBehaviour) {
        enemyBehaviour.rb.MovePosition(enemyBehaviour.rb.position + enemyBehaviour.vectorToPlayer 
                                       * enemyBehaviour.moveSpeed * Time.fixedDeltaTime);
    }

    public override void IsShotState(EnemyBehaviour enemyBehaviour) {}

    public override void ExitState(EnemyBehaviour enemyBehaviour) {}
}