using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWall : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            collider.gameObject.GetComponent<ProjectileBehaviour>().AddState(new BombProjectileState());
        }
    }
}
