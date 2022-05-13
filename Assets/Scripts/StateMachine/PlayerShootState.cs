using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        Debug.Log("SHOOT");
        playerMovement.shooter.spawnProj();
        playerMovement.SwitchState(playerMovement.RunState);
    }

    public override void UpdateState(PlayerMovement playerMovement) {}

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {}

    public override void ExitState(PlayerMovement playerMovement) {}
}
