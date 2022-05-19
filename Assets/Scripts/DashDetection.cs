using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDetection : MonoBehaviour
{
    public PlayerMovement player;
    public ShooterBehaviour shooter;

    void OnTriggerEnter(Collider collider) {
        Debug.Log("Dashed over something");
        
        // if dashed over enemy
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            // damage enemy
            collider.gameObject.GetComponent<EnemyBehaviour>().health -= player.jumpingDamage;
            // if enemy is below 0 health
            if (collider.gameObject.GetComponent<EnemyBehaviour>().health <= 0) {
                // kill enemy (important to remove from enemies)
                shooter.enemies.Remove(collider.gameObject);
                Destroy(collider.gameObject);
            }
        }
    }
}
