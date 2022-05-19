using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootState : PlayerBaseState
{
    public override void EnterState(PlayerMovement playerMovement) {
        // debugging
        playerMovement.rend.material.color = Color.red;
        Debug.Log("SHOOT");
        // spaw projectile and immediately return to run
        playerMovement.shooter.spawnProj();
        playerMovement.SwitchState(playerMovement.RunState);
    }

    public override void UpdateState(PlayerMovement playerMovement) {}

    public override void CheckSwitchState(PlayerMovement playerMovement) {}

    public override void InitializeSubState(PlayerMovement playerMovement) {}

    public override void FixedUpdateState(PlayerMovement playerMovement) {}

    public override void ExitState(PlayerMovement playerMovement) {}
}
