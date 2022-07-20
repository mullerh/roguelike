using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterBehaviour : MonoBehaviour
{
    public PlayerControls playerControls;
    public PlayerMovement playerMovement;
    public ProjectileBehaviour projectilePrefab;
    public float fireRate;
    public float lockingAngleRange;
    public List<GameObject> enemies;

    private Vector3 shootDirection;
    private Transform playerTransform;
    private InputAction shoot;
    private float lastShotTime = -10;
    private Vector3 offsetVector;
    private int wallPlayerLayer;

    // objects
    // private GameObject closestObject = null;

    void Awake() {
        // this must come before playerControls (for some reason??)
        playerTransform = playerMovement.transform;
        // keep a list of all enemies, istead of doing so at each frame
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        playerControls = new PlayerControls();
        // vector from playerTransform to shooter
        offsetVector = transform.position - playerTransform.position;
        // layers the prediction line can hit
        wallPlayerLayer = LayerMask.GetMask("Walls");
    }

    void OnEnable() {
        // allow shooter to handle fire (shoot) inputs
        shoot = playerControls.Player.Fire;
        shoot.Enable();
    }

    void OnDisable() {
        shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // get direction of shoot input
        Vector2 tempDir = shoot.ReadValue<Vector2>();
        shootDirection = new Vector3(tempDir.x, 0, tempDir.y);
        if (shootDirection != Vector3.zero) {
            shootDirection.Normalize();

            // adjust for camera angle
            float targetAngle = Mathf.Atan2(shootDirection.x, shootDirection.z) * Mathf.Rad2Deg + playerMovement.cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            shootDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            shootDirection.Normalize();
        }

        // get the enemy in aim FOV that is closest to the line extending out from the direction the playerTransform is facing
        GameObject closestEnemyInRange = getClosestEnemiesInRange(new Vector2(shootDirection.x, shootDirection.z));
        // // if such an enemy exists, override the direction playerTransform is shooting (otherwise will stay the direction of input)
        if (closestEnemyInRange != null) {
            shootDirection = closestEnemyInRange.transform.position - playerTransform.position;
            shootDirection.Normalize();

            Debug.Log(Vector3.Scale(Mathf.Sqrt(3) * shootDirection, offsetVector));
        }

        // move shooter towards the direction playerTransform is shooting (while maintaining offset XY ratio)
        transform.position = Vector3.Scale(Mathf.Sqrt(3) * shootDirection, offsetVector) + playerTransform.position;
    }

    public void spawnProj(){
        // if it has been long enough since last shot
        if (lastShotTime + 1/fireRate < Time.time) {
            lastShotTime = Time.time;
            
            // TODO: move to factory pattern
            // create a projectile at position of the shooter
            ProjectileBehaviour newProj = Instantiate(projectilePrefab, 
                                                       transform.position, 
                                                       Quaternion.identity).GetComponent<ProjectileBehaviour>();

            newProj.shooterMoveSpeed = playerMovement.moveSpeed;
            newProj.shootDirection = shootDirection;
        }
    }

    // REQUIRES: shooter offset X to Y ratio to be 1:1
    private GameObject getClosestEnemiesInRange(Vector2 direction) {
        // left and right FOV of the shooter's autoassist
        Vector2 lv = Rotate(direction, -lockingAngleRange);
        Vector2 rv = Rotate(direction, lockingAngleRange);
        
        Vector2 playerPos2D = new Vector2(playerTransform.position.x, playerTransform.position.z);

        // for debugging
        Debug.DrawLine(playerTransform.position, new Vector3(playerTransform.position.x + lv.x, playerTransform.position.y, playerTransform.position.z + lv.y));
        Debug.DrawLine(playerTransform.position, new Vector3(playerTransform.position.x + rv.x, playerTransform.position.y, playerTransform.position.z + rv.y));

        // ray from playerTransform going towards the direction of shot
        Ray ray = new Ray(playerPos2D, direction);
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject enemy in enemies) {
            Transform enemyTransform = enemy.transform;
            Vector2 enemyPos2D = new Vector2(enemyTransform.position.x, enemyTransform.position.z);

            // FROM: https://stackoverflow.com/questions/11456671/determine-if-a-point-is-within-the-range-of-two-other-points-that-create-infinit
            Vector2 enemyDirection = enemyPos2D - playerPos2D;
            float Sl = lv.y * enemyDirection.x - lv.x * enemyDirection.y;
            float Sr = -rv.y * enemyDirection.x + rv.x * enemyDirection.y;

            // if enemy is within autoaim FOV 
            if (Sl < 0 && Sr < 0) {
                // if line from enemy to playerTransform is unobstructed
                if (!Physics.Linecast(enemyTransform.position, playerTransform.position, wallPlayerLayer)) {
                    Debug.DrawLine(enemyTransform.position, playerTransform.position);
                    // FROM: https://answers.unity.com/questions/568773/shortest-distance-from-a-point-to-a-vector.html
                    // distance from enemy to ray
                    float currentDistance = Vector3.Cross(ray.direction, enemyPos2D - (Vector2)ray.origin).magnitude;
                    if (currentDistance < closestDistance) {
                        closestDistance = currentDistance;
                        closestObject = enemy;
                    }
                }
            }
        }
        return closestObject;
    }

    // FROM: https://stackoverflow.com/questions/22818531/how-to-rotate-2d-vector
    private const float DegToRad = Mathf.PI/180;

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        return RotateRadians(v, degrees * DegToRad);
    }

    public static Vector2 RotateRadians(Vector2 v, float radians)
    {
        float ca = Mathf.Cos(radians);
        float sa = Mathf.Sin(radians);
        return new Vector2(ca*v.x - sa*v.y, sa*v.x + ca*v.y);
    }
}
