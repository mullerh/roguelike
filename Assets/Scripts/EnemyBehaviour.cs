using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // stats
    public float health;
    public float damage;
    public float followThreshold;
    public float moveSpeed;

    // objects
    public Rigidbody2D rb;
    private GameObject player;
    private Transform playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() { 
        Vector2 vectorToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;
        vectorToPlayer.Normalize();
        if (distanceToPlayer < followThreshold) {
            rb.MovePosition(rb.position + vectorToPlayer * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            health -= collision.gameObject.GetComponent<ProjectileBehaviour>().damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
