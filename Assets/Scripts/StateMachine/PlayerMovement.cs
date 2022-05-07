using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // controls
    public PlayerControls playerControls;

    // game objects
    public GameObject obstacles;

    // states
    private PlayerBaseState currentState;
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerShootState ShootState = new PlayerShootState();

    // movement
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Vector2 moveDirection = Vector2.zero;

    // inputs
    public InputAction move;
    public InputAction jump;
    public InputAction shoot;

    // jump
    public float jumpTime = 0.5f;
    public float jumpCooldown = 3;
    public float jumpSpeed = 2f;
    public float jumpTimer = -1000;
    public float cooldownUntilNextJump = -3;

    // shoot
    public ProjectileBehaviour projectilePrefab;
    public Transform projectileLaunchOffset;
    public float fireRate;
    private Vector2 offsetVector = new Vector2(1, 1);
    private float lastShotTime = -10;

    // player stats
    public float health;
    // TODO: Dash damage

    void Awake() {
        playerControls = new PlayerControls();
        obstacles = GameObject.FindGameObjectWithTag("Obstacles");

        offsetVector.Normalize();
        offsetVector = offsetVector * (projectileLaunchOffset.position - transform.position).magnitude;
    }

    void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
        jump = playerControls.Player.Jump;
        jump.Enable();
        shoot = playerControls.Player.Fire;
        shoot.Enable();
    }

    void OnDisable() {
        move.Disable();
        jump.Disable();
        shoot.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveDirection.x = 1;
        currentState = IdleState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        projectileLaunchOffset.position = Vector2.Scale(Mathf.Sqrt(2) * moveDirection, offsetVector) + (Vector2)transform.position;
    }

    public void SwitchState(PlayerBaseState state) {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);

    }

    void FixedUpdate() 
    {
        currentState.FixedUpdateState(this);
    }

    public void spawnProj(){
        if (lastShotTime + 1/fireRate < Time.time) {
            lastShotTime = Time.time;
            
            Instantiate(projectilePrefab, projectileLaunchOffset.position, Quaternion.identity);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            health -= collision.gameObject.GetComponent<EnemyBehaviour>().damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
