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
    private PlayerMovement playerMovement;

    // shooter
    private ShooterBehaviour shooter;

    // states
    public List<BaseProjectileState> projStates = new List<BaseProjectileState>();

    // gravity
    public float gravityMultipler = 1.125f;

    void Start() { 
        // get scripts of player and shooter
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        shooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<ShooterBehaviour>();

        if (travelVector == new Vector3(0, 0, 0)) {
            travelVector = (projSpeed + playerMovement.moveSpeed) * shooter.shootDirection;
        }
    }

    public void AddState(BaseProjectileState projState) {
        projState.ProjectileStateAdded(this);
        projStates.Add(projState);
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
