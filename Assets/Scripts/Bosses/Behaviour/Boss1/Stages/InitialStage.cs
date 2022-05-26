using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStage : BaseStage
{
    public override void EnterStage(Boss1Behaviour bossBehaviour) {
        bossBehaviour.rend.material.color = Color.yellow;
    }

    public override void UpdateStage(Boss1Behaviour bossBehaviour) {

    }

    public override void FixedUpdateStage(Boss1Behaviour bossBehaviour) {

    }

    public override void ExitStage(Boss1Behaviour bossBehaviour) {

    }
}
