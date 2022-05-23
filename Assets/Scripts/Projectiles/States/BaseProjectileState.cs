using UnityEngine;

public abstract class BaseProjectileState
{
    public abstract void ProjectileStateAdded(ProjectileBehaviour pb);

    public abstract void ProjectileStateFixedUpdate(ProjectileBehaviour pb);

    public abstract void ProjectileStateOnCollisionEnter(ProjectileBehaviour pb, Collision collision);
}
