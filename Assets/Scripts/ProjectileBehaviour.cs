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
    private Vector3 initVel = Vector3.zero;

    // shooter
    private ShooterBehaviour shooter;

    void Start() {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        shooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<ShooterBehaviour>();

        initVel.x = shooter.shootDirection.x;
        initVel.y = shooter.shootDirection.y;

        if (initVel.y == 0 && initVel.x == 0) {
            initVel.x = 1;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition((Vector3)rb.position + (Time.deltaTime * (projSpeed + playerMovement.moveSpeed) * initVel));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
