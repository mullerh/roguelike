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
    
    void Awake() {
        vectorToPlayer = Vector2.zero;
        distanceToPlayer = Mathf.Infinity;
        obstacles = GameObject.FindGameObjectWithTag("Obstacles");
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        wallPlayerLayer = LayerMask.GetMask("Player", "Walls", "Jumper");
        shooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<ShooterBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.position, wallPlayerLayer);

        if (hit) {
            if (hit.transform.tag == "Player") {
                vectorToPlayer = playerTransform.position - transform.position;
                distanceToPlayer = vectorToPlayer.magnitude;
                vectorToPlayer.Normalize();
            }
        } else {
            vectorToPlayer = Vector2.zero;
        }

        currentState.UpdateState(this);
    }

    private void FixedUpdate() { 
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(EnemyBaseState state) {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            health -= collision.gameObject.GetComponent<ProjectileBehaviour>().damage;
            if (health <= 0) {
                // int index = Array.IndexOf(array, gameObject);
                // shooter.enemies = shooter.enemies.Where((e, i) => i != index).ToArray();
                shooter.enemies.Remove(gameObject);
                Destroy(gameObject);
            }
            currentState.IsShotState(this);
        }
    }
}
