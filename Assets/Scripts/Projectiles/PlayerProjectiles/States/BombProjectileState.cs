using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectileState : BaseProjectileState
{
    public override void ProjectileStateAdded(ProjectileBehaviour pb)
    {
        pb.transform.localScale *= 2f;
    }

    public override void ProjectileStateFixedUpdate(ProjectileBehaviour pb)
    {
        pb.rb.AddForce((Vector3.down * pb.gravityMultipler), ForceMode.Acceleration);
    }

    public override void ProjectileStateOnCollisionEnter(ProjectileBehaviour pb, Collision collision) {
        // TODO
    }
}
