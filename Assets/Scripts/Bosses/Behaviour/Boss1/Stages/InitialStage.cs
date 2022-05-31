using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStage : BaseStage
{ 
    private float rotationCycle = 0;
    private float rotationPerCycle = 5;
    private float splitAngle;

    public override void EnterStage(Boss1Behaviour bossBehaviour) {
        bossBehaviour.rend.material.color = Color.yellow;

        splitAngle = 360 / bossBehaviour.numberOfSplits;
    }

    public override void UpdateStage(Boss1Behaviour bossBehaviour) {
        if (bossBehaviour.lastShotTime + 1/bossBehaviour.baseFireRate < Time.time) {
            if (rotationCycle > rotationPerCycle) {
                rotationCycle = 0;
            }
            rotationCycle++;
            float startAngle = rotationCycle * (splitAngle / (rotationPerCycle + 1));
 
            for (float i = startAngle; i < 360 + startAngle; i += splitAngle) {
                Debug.Log(i);
                bossBehaviour.spawnProj(Quaternion.Euler(0, i, 0) * (Vector3.right + Vector3.forward));
            }
        }
    }

    public override void FixedUpdateStage(Boss1Behaviour bossBehaviour) {
    }

    public override void ExitStage(Boss1Behaviour bossBehaviour) {

    }
}
