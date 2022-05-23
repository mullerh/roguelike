using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectileState : BaseProjectileState
{
    public override void ProjectileStateAdded(ProjectileBehaviour pb)
    {
        pb.damage += 5;
    }

    public override void ProjectileStateFixedUpdate(ProjectileBehaviour pb)
    {
        pb.damage += 5;
    }

    public override void ProjectileStateOnCollisionEnter(ProjectileBehaviour pb, Collision collision) {
        // TODO
    }
}
