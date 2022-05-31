using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // DEBUG
    public float numberOfStates;

    // objects
    public Rigidbody rb;

    // stats
    public float projSpeed;
    public float damage;
    public bool isParent = true;

    // movement
    public Vector3 travelVector;

    // MUST BE SET
    [HideInInspector] public float shooterMoveSpeed;
    [HideInInspector] public Vector3 shootDirection;

    public List<BaseProjectileState> projStates = new List<BaseProjectileState>();

    // gravity
    public float gravityMultipler = 1.125f;

    void Start() { 
        if (travelVector == new Vector3(0, 0, 0)) {
            travelVector = (projSpeed + shooterMoveSpeed) * shootDirection;
        }
    }

    public void AddState(BaseProjectileState projState) {
        projStates.Add(projState);
        projState.ProjectileStateAdded(this);
    }

    private void FixedUpdate()
    {
        // move the projectile in the initial shooter direction at speed (max player speed + projectile speed)
        // (maybe?? test out using length of: player movement direction projected onto initDirection (something like binding of isaac))
        rb.MovePosition(rb.position + (Time.deltaTime * travelVector));
        foreach (BaseProjectileState state in projStates) {
            state.ProjectileStateFixedUpdate(this);
        }
        numberOfStates = projStates.Count;
    }

    // destroy when it collides with something
    private void OnCollisionEnter(Collision collision) {
        foreach (BaseProjectileState state in projStates) {
            state.ProjectileStateOnCollisionEnter(this, collision);
        }
        Destroy(gameObject);
    }
}
