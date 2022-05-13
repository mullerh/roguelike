using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyBehaviour enemyBehaviour) {}

    public override void UpdateState(EnemyBehaviour enemyBehaviour) {
        if (enemyBehaviour.distanceToPlayer < enemyBehaviour.agroThreshold) {
            enemyBehaviour.SwitchState(enemyBehaviour.AgroState);
        }
    }

    public override void FixedUpdateState(EnemyBehaviour enemyBehaviour) {}

    public override void IsShotState(EnemyBehaviour enemyBehaviour) {
        enemyBehaviour.SwitchState(enemyBehaviour.JumpingState);
    }

    public override void ExitState(EnemyBehaviour enemyBehaviour) {}
}
