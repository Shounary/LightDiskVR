using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : Weapon
{

    public Transform rotateTransform;
    public List<string> noStickTags = new List<string>();
    public float rotateRate = 5f;
    public float coolDownTime = 3f;

    private bool isHeldLag;
    private bool thrown;
    private float currCoolDownTime;

    private void OnCollisionEnter(Collision other) {
        GameObject otherGO = other.gameObject;
        if(!noStickTags.Contains(otherGO.tag))
        {
            weaponRB.velocity = Vector3.zero;
            weaponRB.isKinematic = true;
        }
        
    }

    public override void TriggerFunction(float additionalFactor, Transform targetTransform) {
        if ((thrown && Time.fixedUnscaledTime - currCoolDownTime >= coolDownTime) || !isHeld) {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isHeldLag = isHeld;
        thrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHeld && isHeldLag) {
            thrown = true;
            currCoolDownTime = Time.fixedUnscaledTime;
        }
        isHeldLag = isHeld;
    }

    void FixedUpdate() {
        if (thrown) {
            rotateTransform.Rotate(0f, 0f, rotateRate);
        }
    }

}