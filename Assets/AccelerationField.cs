using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AccelerationField : MonoBehaviour
{
    public float playerEffectSpeed = 5f;
    public LayerMask playerLayerMask;

    private float[] teleportBoundsX = {-5f, 5f};
    private float[] teleportBoundsZ = {-10f, 10f};
    private Vector3 forceVector = Vector3.right;

    private void OnTriggerStay(Collider other) {

        if ((playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            // if the player is to be teleported
            XRRig rig = other.transform.gameObject.GetComponentInParent<XRRig>();

            float x = playerEffectSpeed * forceVector.normalized.x * Time.deltaTime;
            float z = playerEffectSpeed * forceVector.normalized.z * Time.deltaTime;
            
            // limit the player teleport to the given bounds
            if (rig.transform.position.x + x < teleportBoundsX[0] || rig.transform.position.x + x > teleportBoundsX[1])
                x = 0f;
            if (rig.transform.position.z + z < teleportBoundsZ[0] || rig.transform.position.z + z > teleportBoundsZ[1])
                z = 0f;
                            
            rig.transform.Translate(new Vector3(x, 0f, z), Space.World);


        } else if (other.GetComponent<Rigidbody>() != null) {
            // if the physics object is to be affected
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(forceVector.normalized / 2, ForceMode.Force);
        }
    }

    public void SetForceVector(Vector3 forceVector) {
        this.forceVector = forceVector;
    }

    public void SetBounds(float[] boundsX, float[] boundsZ) {
        teleportBoundsX = boundsX;
        teleportBoundsZ = boundsZ;
    }
}
