using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : Weapon
{
    /*public List<string> noStickTags = new List<string>();
    public bool hasCollided = true;

    private void OnCollisionEnter(Collision other) {
        GameObject otherGO = other.gameObject;
        if(!noStickTags.Contains(otherGO.tag))
        {
            //this.gameObject.transform.SetParent(otherGO.transform);
            hasCollided = true;
            weaponRB.velocity = Vector3.zero;
            weaponRB.isKinematic = true;
        }
        
    }

    public override void TriggerPressFunction()
    {
        if(hasCollided)
        {
            this.gameObject.transform.SetPositionAndRotation(tar, targetTransform.rotation);
           // this.gameObject.transform.SetParent(parentGameObject.transform);
            hasCollided = false;
            weaponRB.isKinematic = false;
        }
    }*/

}
