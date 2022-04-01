using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationField : MonoBehaviour
{
    private Vector3 forceVector = Vector3.right;

    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Rigidbody>() != null) {
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            otherRB.AddForce(forceVector.normalized / 2, ForceMode.Force);
        }
    }

    public void SetForceVector(Vector3 forceVector) {
        this.forceVector = forceVector;
    }
}
