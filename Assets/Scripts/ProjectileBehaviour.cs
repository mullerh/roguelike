using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // objects
    public Rigidbody2D rb;
    public GameObject player;

    // stats
    public float projSpeed;
    public float damage;

    // movement
    private PlayerMovement playerMovement;
    private Vector3 initVel = Vector3.zero;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        initVel.x = playerMovement.moveDirection.x;
        initVel.y = playerMovement.moveDirection.y;

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
