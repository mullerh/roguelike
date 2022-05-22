using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // controls
    [HideInInspector] public PlayerControls playerControls;

    // game objects
    [HideInInspector] public GameObject obstacles;

    // states
    private PlayerBaseState currentState;
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerShootState ShootState = new PlayerShootState();

    // movement
    public Rigidbody rb;
    public float moveSpeed = 5f;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;

    // inputs
    [HideInInspector] public InputAction move;
    [HideInInspector] public InputAction jump;
    [HideInInspector] public InputAction shoot;

    // jump
    public float jumpTime = 0.5f;
    public float jumpCooldown = 3;
    public float jumpSpeed = 2f;
    public float jumpingDamage;
    public GameObject dashWallPrefab;
    [HideInInspector] public float cooldownUntilNextJump = -3;

    // shoot
    public ShooterBehaviour shooter;

    // player stats
    public float health;

    // Debugger variables
    [HideInInspector] public Renderer rend;

    void Awake() {
        // initialize vars
        playerControls = new PlayerControls();
        obstacles = GameObject.FindGameObjectWithTag("Obstacles");
        rend = GetComponent<Renderer>();
    }

    void OnEnable() {
        // enable controls
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
        // start at idle state
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        // handoff update to current state
        currentState.UpdateState(this);
    }

    // handle state switching
    public void SwitchState(PlayerBaseState state) {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);

    }

    void FixedUpdate() 
    {
        // handoff fixedupdate to current state
        currentState.FixedUpdateState(this);
    }


    private void OnCollisionEnter(Collision collision) {
        // keep track of enemies hitting the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            health -= collision.gameObject.GetComponent<EnemyBehaviour>().damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }

    public void drawDashWall(Vector3 startDashPos, Vector3 endDashPos) {
        // Instantiate(dashWallPrefab, middleDashPos, Quaternion.LookRotation(endDashPos));
        GameObject dashWall = Instantiate(dashWallPrefab);
        dashWall.transform.localScale = new Vector3(0.25f, 1f, Vector3.Distance(startDashPos, endDashPos));
        dashWall.transform.position = startDashPos + (endDashPos - startDashPos)/2;
        dashWall.transform.rotation = Quaternion.LookRotation(endDashPos - startDashPos);
    }
}
