using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDetection : MonoBehaviour
{
    public PlayerMovement player;
    public ShooterBehaviour shooter;

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("AAAAAAAAAAAAAAAAAA");
        if (player.gameObject.layer == LayerMask.NameToLayer("Jumper")) {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
                collider.gameObject.GetComponent<EnemyBehaviour>().health -= player.jumpingDamage;
                if (collider.gameObject.GetComponent<EnemyBehaviour>().health <= 0) {
                    shooter.enemies.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                }
            }
        }
    }
}
