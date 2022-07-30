using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStage : BaseStage
{ 
    private float splitAngle;
    private float startAngle = 0;
    private float leftOverAngle = 0;

    public override void EnterStage(Boss1Behaviour bossBehaviour) {
        bossBehaviour.rend.material.color = Color.yellow;

        splitAngle = 360 / bossBehaviour.numberOfSplits;
    }

    public override void UpdateStage(Boss1Behaviour bossBehaviour) {
        if (bossBehaviour.lastShotTime + 1/bossBehaviour.baseFireRate < Time.time) {
            startAngle = (splitAngle * bossBehaviour.rotationPerCycle) + leftOverAngle;
            Debug.Log(startAngle);
 
            float i;
            for (i = startAngle; i < 360 + startAngle; i += splitAngle) {
                bossBehaviour.spawnProj(Quaternion.Euler(0, i, 0) * (Vector3.right + Vector3.forward));
            }
            leftOverAngle = i % 360;
        }
    }

    public override void FixedUpdateStage(Boss1Behaviour bossBehaviour) {
    }

    public override void ExitStage(Boss1Behaviour bossBehaviour) {

    }
}
