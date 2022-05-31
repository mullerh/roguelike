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
                ProjectileBehaviour colliderProjBehaviour = collider.gameObject.GetComponent<ProjectileBehaviour>();

                for (float i = spreadAngle / numberOfSplits; i <= spreadAngle; i += spreadAngle / numberOfSplits) {
                    // TODO move Instantiate to factory pattern
                    GameObject newProjPos = Instantiate(collider.gameObject, collider.gameObject.transform.position, Quaternion.identity);
                    ProjectileBehaviour posProjBehaviour = newProjPos.GetComponent<ProjectileBehaviour>();

                    Physics.IgnoreCollision(newProjPos.GetComponent<SphereCollider>(), transform.root.GetComponent<BoxCollider>());
                    posProjBehaviour.travelVector = Quaternion.Euler(0, i, 0) * colliderProjBehaviour.travelVector;
                    posProjBehaviour.isParent = false;
                    posProjBehaviour.projStates = new List<BaseProjectileState>(colliderProjBehaviour.projStates);

                    // TODO move Instantiate to factory pattern
                    GameObject newProjNeg = Instantiate(collider.gameObject, collider.gameObject.transform.position, Quaternion.identity);
                    ProjectileBehaviour negProjBehaviour = newProjNeg.GetComponent<ProjectileBehaviour>();

                    Physics.IgnoreCollision(newProjNeg.GetComponent<SphereCollider>(), transform.root.GetComponent<BoxCollider>());
                    negProjBehaviour.travelVector = Quaternion.Euler(0, -i, 0) * colliderProjBehaviour.travelVector;
                    negProjBehaviour.isParent = false;
                    negProjBehaviour.projStates = new List<BaseProjectileState>(colliderProjBehaviour.projStates);
                }
            }
        }
    }
}
