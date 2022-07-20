using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehaviour : ProjectileBehaviour
{
    protected override void DealDamage(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            collision.gameObject.GetComponent<DamageableBehaviour>().health -= damage;
            if (collision.gameObject.GetComponent<DamageableBehaviour>().health <= 0) {
                PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                player.shooter.enemies.Remove(collision.gameObject);
                Destroy(collision.gameObject);
                Debug.Log("Enemy Killed");
            }
            Debug.Log("Enemy Hit");
        }
    }
    
}
