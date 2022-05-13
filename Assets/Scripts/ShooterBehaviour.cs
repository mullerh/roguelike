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
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        playerControls = new PlayerControls();
        offsetVector = transform.position - player.position;
        wallPlayerLayer = LayerMask.GetMask("Player", "Walls", "Jumper");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        shoot = playerControls.Player.Fire;
        shoot.Enable();
    }

    void OnDisable() {
        shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        shootDirection = shoot.ReadValue<Vector2>();
        shootDirection.Normalize();

        GameObject closestEnemyInRange = getClosestEnemiesInRange();
        if (closestEnemyInRange != null) {
            shootDirection = closestEnemyInRange.transform.position - player.position;
            shootDirection.Normalize();
        }

        transform.position = Vector2.Scale(Mathf.Sqrt(2) * shootDirection, offsetVector) + (Vector2)player.position;
    }

    public void spawnProj(){
        if (lastShotTime + 1/fireRate < Time.time) {
            lastShotTime = Time.time;
            
            Instantiate(projectilePrefab, 
                        transform.position, 
                        Quaternion.identity);
        }
    }

    private GameObject getClosestEnemiesInRange() {
        Vector2 lv = Rotate(shootDirection, -lockingAngleRange);
        Vector2 rv = Rotate(shootDirection, lockingAngleRange);

        Debug.DrawLine(player.position, (Vector2)player.position + lv);
        Debug.DrawLine(player.position, (Vector2)player.position + rv);

        Ray ray = new Ray(player.position, shootDirection);
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in enemies) {
            Transform enemyTransform = obj.transform;

            // FROM: https://stackoverflow.com/questions/11456671/determine-if-a-point-is-within-the-range-of-two-other-points-that-create-infinit

            Vector2 direction = enemyTransform.position - player.position;
            float Sl = lv.y * direction.x - lv.x * direction.y;
            float Sr = -rv.y * direction.x + rv.x * direction.y;

            if (Sl < 0 && Sr < 0) {
                if (Physics2D.Linecast(enemyTransform.position, player.position, wallPlayerLayer).transform.tag == "Player") {
                    Debug.DrawLine(enemyTransform.position, player.position);
                    // FROM: https://answers.unity.com/questions/568773/shortest-distance-from-a-point-to-a-vector.html
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
