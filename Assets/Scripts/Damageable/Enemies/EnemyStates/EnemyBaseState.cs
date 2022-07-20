using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyBehaviour enemyBehaviour);

    public abstract void UpdateState(EnemyBehaviour enemyBehaviour);

    public abstract void FixedUpdateState(EnemyBehaviour enemyBehaviour);

    public abstract void IsShotState(EnemyBehaviour enemyBehaviour);

    public abstract void ExitState(EnemyBehaviour enemyBehaviour);
}
