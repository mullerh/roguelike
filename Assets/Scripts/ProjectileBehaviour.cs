using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // objects
    public Rigidbody2D rb;

    // stats
    public float projSpeed;
    public float damage;

    // movement
    private PlayerMovement playerMovement;
    private Vector2 initDirection = Vector2.zero;

    // shooter
    private ShooterBehaviour shooter;

    void Start() {
        // get scripts of player and shooter
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        shooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<ShooterBehaviour>();

        // save the initial direction of shooter when projectile is shot
        initDirection.x = shooter.shootDirection.x;
        initDirection.y = shooter.shootDirection.y;

        // if the shooter direction is somehow 0, default to right
        if (initDirection.y == 0 && initDirection.x == 0) {
            initDirection.x = 1;
        }
    }
    private void FixedUpdate()
    {
        // move the projectile in the initial shooter direction at speed (max player speed + projectile speed)
        // (maybe?? test out using length of: player movement direction projected onto initDirection (something like binding of isaac))
        rb.MovePosition(rb.position + (Time.deltaTime * (projSpeed + playerMovement.moveSpeed) * initDirection));
    }

    // destroy when it collides with something
    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
