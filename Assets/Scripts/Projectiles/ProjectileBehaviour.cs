using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    [Header("DEBUG")]
    public float numberOfStates;

    [Header("Objects")]
    public Rigidbody rb;

    [Header("Stats")]
    public float projSpeed;
    public float damage;
    public float range;
    [HideInInspector] public bool isParent = true;

    [Header("Movement")]
    public Vector3 travelVector;

    // MUST BE SET
    [HideInInspector] public float shooterMoveSpeed;
    [HideInInspector] public Vector3 shootDirection;

    public List<BaseProjectileState> projStates = new List<BaseProjectileState>();

    [Header("Gravity")]
    public float gravityMultipler;

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
        //this should be moved elsewhere
        numberOfStates = projStates.Count;

        gameObject.transform.localScale -= ((1 / range) *  Time.deltaTime) * new Vector3(1, 1, 1);

        if (gameObject.transform.localScale.x < 1/4) {
            Destroy(gameObject);
        }
    }

    // destroy when it collides with something
    private void OnCollisionEnter(Collision collision) {
        foreach (BaseProjectileState state in projStates) {
            state.ProjectileStateOnCollisionEnter(this, collision);
        }
        DealDamage(collision);
        Destroy(gameObject);
    }

    protected abstract void DealDamage(Collision collision);
}
