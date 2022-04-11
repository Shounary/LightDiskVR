using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AccelerationField : MonoBehaviour
{
    public float playerEffectSpeed = 5f;
    public LayerMask playerLayerMask;
    private Vector3 forceVector = Vector3.right;

    private void OnTriggerStay(Collider other) {
        if ((playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            XRRig rig = other.transform.gameObject.GetComponentInParent<XRRig>();
            rig.transform.Translate(playerEffectSpeed * forceVector.normalized * Time.deltaTime, Space.World);
        } else if (other.GetComponent<Rigidbody>() != null) {
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(forceVector.normalized / 2, ForceMode.Force);
        }
    }

    public void SetForceVector(Vector3 forceVector) {
        this.forceVector = forceVector;
    }
}
