using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ProjectileBehaviour : ProjectileBehaviour
{
    protected override void DealDamage(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collision.gameObject.GetComponent<PlayerMovement>().health -= damage;
            if (collision.gameObject.GetComponent<PlayerMovement>().health <= 0) {
                Destroy(collision.gameObject);
                Debug.Log("Player Killed");
            }
            Debug.Log("Player Hit");
        }
    }
}
