using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // stats
    public float health;
    public float damage;
    public float agroThreshold;
    public float stopAgroThreshold;
    public float moveSpeed;

    // objects
    public Rigidbody2D rb;
    private Transform playerTransform;
    [HideInInspector] public GameObject obstacles;
    private int wallPlayerLayer;
    private ShooterBehaviour shooter;

    // states
    [SerializeField] public EnemyAgroState AgroState = new EnemyAgroState();
    public EnemyIdleState IdleState = new EnemyIdleState();
    public EnemyJumpingState JumpingState = new EnemyJumpingState();
    private EnemyBaseState currentState;

    // player vectors
    [HideInInspector] public Vector2 vectorToPlayer;
    [HideInInspector] public float distanceToPlayer;

    // jump
    public float jumpLength;
    public float jumpSpeed;

    // Debugger variables
    [HideInInspector] public SpriteRenderer enemySprite;
    
    void Awake() {
        // for debugging
        enemySprite = GetComponent<SpriteRenderer>();

        vectorToPlayer = Vector2.zero;
        distanceToPlayer = Mathf.Infinity;
        obstacles = GameObject.FindGameObjectWithTag("Obstacles");
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        wallPlayerLayer = LayerMask.GetMask("Player", "Walls", "Jumper");

        // so it can remove itself from shooter's enemy list if killed
        shooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<ShooterBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // start in idle state
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //BUG: if there is a dashed enemy between player and enemy, then it will briefly stop following
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.position, wallPlayerLayer);

        // if something was hit by line (only really a problem when player is dashed over the enemy)
        if (hit) {
            // and if thing was player, update variables to point towards player
            if (hit.transform.tag == "Player") {
                vectorToPlayer = playerTransform.position - transform.position;
                distanceToPlayer = vectorToPlayer.magnitude;
                vectorToPlayer.Normalize();
            }
        } else {
            // if nothing was hit by line, default to being stationary (dont have to move if player is overtop)
            vectorToPlayer = Vector2.zero;
        }

        // hand off updating to current state
        currentState.UpdateState(this);
    }

    private void FixedUpdate() {
        // hand off to current state
        currentState.FixedUpdateState(this);
    }

    // handle switching states
    public void SwitchState(EnemyBaseState state) {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    // enemies handle collisions with projectiles for performance (worst case for number of projectiles is way higher)
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            health -= collision.gameObject.GetComponent<ProjectileBehaviour>().damage;
            if (health <= 0) {
                shooter.enemies.Remove(gameObject);
                Destroy(gameObject);
            }
            currentState.IsShotState(this);
        }
    }
}
