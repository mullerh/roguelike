using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyBehaviour enemyBehaviour) {
        // debugging
        enemyBehaviour.enemySprite.color = Color.white;
    }

    public override void UpdateState(EnemyBehaviour enemyBehaviour) {
        // switch to agro if player gets close enough
        if (enemyBehaviour.distanceToPlayer < enemyBehaviour.agroThreshold) {
            enemyBehaviour.SwitchState(enemyBehaviour.AgroState);
        }
    }

    public override void FixedUpdateState(EnemyBehaviour enemyBehaviour) {}

    public override void IsShotState(EnemyBehaviour enemyBehaviour) {
        // jump at player if shot
        enemyBehaviour.SwitchState(enemyBehaviour.JumpingState);
    }

    public override void ExitState(EnemyBehaviour enemyBehaviour) {}
}
