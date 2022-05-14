using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterBehaviour : MonoBehaviour
{
    public PlayerControls playerControls;
    public Transform player;
    public ProjectileBehaviour projectilePrefab;
    public float fireRate;
    public float lockingAngleRange;
    [HideInInspector] public Vector2 shootDirection;
    private InputAction shoot;
    private float lastShotTime = -10;
    private Vector2 offsetVector;
    private int wallPlayerLayer;
    public List<GameObject> enemies;

    // objects
    // private GameObject closestObject = null;

    void Awake() {
        // keep a list of all enemies, istead of doing so at each frame
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        playerControls = new PlayerControls();
        // vector from player to shooter
        offsetVector = transform.position - player.position;
        // layers the prediction line can hit
        wallPlayerLayer = LayerMask.GetMask("Player", "Walls");
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
        shootDirection = shoot.ReadValue<Vector2>();
        shootDirection.Normalize();

        // get the enemy in aim FOV that is closest to the line extending out from the direction the player is facing
        GameObject closestEnemyInRange = getClosestEnemiesInRange();
        // if such an enemy exists, override the direction player is shooting (otherwise will stay the direction of input)
        if (closestEnemyInRange != null) {
            shootDirection = closestEnemyInRange.transform.position - player.position;
            shootDirection.Normalize();
        }

        // move shooter towards the direction player is shooting (while maintaining offset XY ratio)
        transform.position = Vector2.Scale(Mathf.Sqrt(2) * shootDirection, offsetVector) + (Vector2)player.position;
    }

    public void spawnProj(){
        // if it has been long enough since last shot
        if (lastShotTime + 1/fireRate < Time.time) {
            lastShotTime = Time.time;
            
            // create a projectile at position of the shooter
            Instantiate(projectilePrefab, 
                        transform.position, 
                        Quaternion.identity);
        }
    }

    // REQUIRES: shooter offset X to Y ratio to be 1:1
    private GameObject getClosestEnemiesInRange() {
        // left and right FOV of the shooter's autoassist
        Vector2 lv = Rotate(shootDirection, -lockingAngleRange);
        Vector2 rv = Rotate(shootDirection, lockingAngleRange);

        // for debugging
        Debug.DrawLine(player.position, (Vector2)player.position + lv);
        Debug.DrawLine(player.position, (Vector2)player.position + rv);

        // ray from player going towards the direction of shot
        Ray ray = new Ray(player.position, shootDirection);
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in enemies) {
            Transform enemyTransform = obj.transform;

            // FROM: https://stackoverflow.com/questions/11456671/determine-if-a-point-is-within-the-range-of-two-other-points-that-create-infinit
            Vector2 enemyDirection = enemyTransform.position - player.position;
            float Sl = lv.y * enemyDirection.x - lv.x * enemyDirection.y;
            float Sr = -rv.y * enemyDirection.x + rv.x * enemyDirection.y;

            // if enemy is within autoaim FOV 
            if (Sl < 0 && Sr < 0) {
                // if line from enemy to player is unobstructed
                if (Physics2D.Linecast(enemyTransform.position, player.position, wallPlayerLayer).transform.tag == "Player") {
                    Debug.DrawLine(enemyTransform.position, player.position);
                    // FROM: https://answers.unity.com/questions/568773/shortest-distance-from-a-point-to-a-vector.html
                    // distance from enemy to ray
                    float currentDistance = Vector3.Cross(ray.direction, obj.transform.position - ray.origin).magnitude;
                    if (currentDistance < closestDistance) {
                        closestDistance = currentDistance;
                        closestObject = obj;
                    }
                }
            }
        }
        return closestObject;
    }

    // // FROM: https://answers.unity.com/questions/568773/shortest-distance-from-a-point-to-a-vector.html
    // private GameObject getClosestEnemy(List<GameObject> points) {
    //     if (points.Count > 0) {
    //         Ray ray = new Ray(player.position, shootDirection);
    //         float closestDistance = Mathf.Infinity;
    //         foreach (GameObject obj in points) {
    //             float currentDistance = Vector3.Cross(ray.direction, obj.GetComponent<Transform>().position - ray.origin).magnitude;
    //             if (currentDistance < closestDistance) {
    //                 closestDistance = currentDistance;
    //                 closestObject = obj;
    //             }
    //         }
    //         return closestObject;
    //     }
    //     return null;
    // }

    // private GameObject getClosestEnemiesInRange() {
    //     Vector2 lv = Rotate(shootDirection, -lockingAngleRange);
    //     Vector2 rv = Rotate(shootDirection, lockingAngleRange);

    //     Debug.DrawLine(player.position, (Vector2)player.position + lv);
    //     Debug.DrawLine(player.position, (Vector2)player.position + rv);

    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //     Ray ray = new Ray(player.position, shootDirection);
    //     float closestDistance = Mathf.Infinity;

    //     int wallLayer = LayerMask.GetMask("Player", "Walls");

    //     foreach (GameObject obj in enemies) {
    //         Transform enemyTransform = obj.GetComponent<Transform>();

    //         // FROM: https://stackoverflow.com/questions/11456671/determine-if-a-point-is-within-the-range-of-two-other-points-that-create-infinit

    //         Vector2 direction = enemyTransform.position - player.position;
    //         float Sl = lv.y * direction.x - lv.x * direction.y;
    //         float Sr = -rv.y * direction.x + rv.x * direction.y;

    //         if (Sl < 0 && Sr < 0) {
    //             if (Physics2D.Linecast(enemyTransform.position, player.position, wallLayer).transform.tag == "Player") {
    //                 Debug.DrawLine(enemyTransform.position, player.position);
    //                 float currentDistance = Vector3.Cross(ray.direction, obj.transform.position - ray.origin).magnitude;
    //                 if (currentDistance < closestDistance) {
    //                     closestDistance = currentDistance;
    //                     closestObject = obj;
    //                 }
    //             }
    //         }
    //     }
    //     return closestObject;
    // }

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
