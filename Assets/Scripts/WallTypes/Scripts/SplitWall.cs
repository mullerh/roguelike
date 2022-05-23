using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitWall : MonoBehaviour
{
    public float spreadAngle;
    //REQUIRE: even
    public float numberOfSplits;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Projectiles")) {
            if (collider.gameObject.GetComponent<ProjectileBehaviour>().isParent) {
                Physics.IgnoreCollision(collider, transform.root.GetComponent<BoxCollider>());

                for (float i = spreadAngle / numberOfSplits; i <= spreadAngle; i += spreadAngle / numberOfSplits) {
                    GameObject newProjPos = Instantiate(collider.gameObject, collider.gameObject.transform.position, Quaternion.identity);
                    Physics.IgnoreCollision(newProjPos.GetComponent<SphereCollider>(), transform.root.GetComponent<BoxCollider>());
                    newProjPos.GetComponent<ProjectileBehaviour>().travelVector = Quaternion.Euler(0, i, 0) * collider.gameObject.GetComponent<ProjectileBehaviour>().travelVector;
                    newProjPos.GetComponent<ProjectileBehaviour>().isParent = false;
                    // newProjPos.GetComponent<Rigidbody>().AddForce(collider.attachedRigidbody.velocity);
                    newProjPos.GetComponent<ProjectileBehaviour>().projStates = collider.gameObject.GetComponent<ProjectileBehaviour>().projStates;

                    GameObject newProjNeg = Instantiate(collider.gameObject, collider.gameObject.transform.position, Quaternion.identity);
                    Physics.IgnoreCollision(newProjNeg.GetComponent<SphereCollider>(), transform.root.GetComponent<BoxCollider>());
                    newProjNeg.GetComponent<ProjectileBehaviour>().travelVector = Quaternion.Euler(0, -i, 0) * collider.gameObject.GetComponent<ProjectileBehaviour>().travelVector;
                    newProjNeg.GetComponent<ProjectileBehaviour>().isParent = false;
                    // newProjNeg.GetComponent<Rigidbody>().AddForce(collider.attachedRigidbody.velocity);
                    newProjNeg.GetComponent<ProjectileBehaviour>().projStates = collider.gameObject.GetComponent<ProjectileBehaviour>().projStates;
                }
            }
        }
    }
}
