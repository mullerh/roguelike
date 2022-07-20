using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Behaviour : DamageableBehaviour
{
    // Debugging
    [HideInInspector] public Renderer rend;

    // stages
    private BaseStage currentStage;
    [HideInInspector] public InitialStage initStage = new InitialStage();
    [HideInInspector] public MiddleStage middleStage = new MiddleStage();
    [HideInInspector] public FinalStage finalStage = new FinalStage();

    // stats
    public float baseDamage;
    public float baseFireRate;
    public int numberOfSplits;
    
    // projectiles
    public ProjectileBehaviour projectilePrefab;
    [HideInInspector] public float lastShotTime = -10;

    // objects
    public PlayerMovement player; 

    void Awake() {
        rend = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStage = initStage;
        currentStage.EnterStage(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentStage.UpdateStage(this);
    }

    void FixedUpdate() 
    {
        // handoff fixedupdate to current state
        currentStage.FixedUpdateStage(this);
    }

    // handle state switching
    public void SwitchStage(BaseStage stage) {
        currentStage.ExitStage(this);
        currentStage = stage;
        currentStage.EnterStage(this);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            health -= collision.gameObject.GetComponent<ProjectileBehaviour>().damage;
            if (health <= 0) {
                player.shooter.enemies.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void spawnProj(Vector3 shootDirection){
        // if it has been long enough since last shot
            lastShotTime = Time.time;
            
            // create a projectile at position of the shooter
            ProjectileBehaviour newProj = Instantiate(projectilePrefab, 
                                                      transform.position + shootDirection.normalized * 4f + 0.3f * Vector3.up, 
                                                      Quaternion.identity).GetComponent<ProjectileBehaviour>();
            newProj.shooterMoveSpeed = 0;
            newProj.shootDirection = shootDirection;
    }
}
