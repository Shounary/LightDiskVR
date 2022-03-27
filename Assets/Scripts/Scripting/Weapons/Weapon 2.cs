using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;




public class Weapon : MonoBehaviour
{
    //Visuals
    public Color primaryColor;
    public Color accentColor;


    public int damage;
    public bool isSummonable;
    public List<string> storableLocations;
    public string playerName;
    //public Collider hurtBox; 
    public Hand hand;
    public Rigidbody weaponRB;

    public Transform weaponTransform;
    //public Transform weaponTransformLeft;
    //public Transform weaponTransformRight;
    public List<Transform> transforms = new List<Transform>();

    public bool isHeld;

    public GameObject parentGameObject; //an empty gameobject with uniform scaling that serves as the default parent

    private void Start() {
        parentGameObject = GameObject.FindGameObjectsWithTag("Empty Parent")[0];
        this.gameObject.transform.SetParent(parentGameObject.transform);
    }
    
    public float diskReturnForceMagnitude = 5f;
    public float stoppingFactorMultiplier = 0.2f;


    public virtual void TriggerFunction(float additionalFactor, Transform targetTransform)
    {
        AttractWeapon(additionalFactor, targetTransform);
    }

    //when called, the weapon will be attracted to the target transfrom
    public void AttractWeapon(float additionalFactor, Transform targetTransform) {
        Vector3 targetDirection = Vector3.Normalize(targetTransform.position - weaponRB.position);
        Vector3 initialDirection = Vector3.Normalize(weaponRB.velocity);
        float angle = Vector3.Angle(targetDirection, initialDirection);

        Vector3 normal = additionalFactor * stoppingFactorMultiplier * diskReturnForceMagnitude * Time.deltaTime * (-1) * Vector3.Magnitude(weaponRB.velocity) * Mathf.Abs(Mathf.Sin(Mathf.Abs(angle))) * initialDirection;
        Vector3 parallel = additionalFactor * diskReturnForceMagnitude * Time.deltaTime * targetDirection;

        if (angle > 5) {
            weaponRB.AddForce(normal, ForceMode.VelocityChange);
        }

        weaponRB.AddForce(parallel, ForceMode.VelocityChange);
    }

    public virtual void MainButtonFunction(){}

    public virtual void SecondaryButtonFunction(){}

    public void setHand(Hand h)
    {
        hand = h;
        if(transforms.Count > 0)
        {
            weaponTransform.position = transforms[(int) h].position;
            weaponTransform.rotation = transforms[(int) h].rotation;
        }
    }

    public virtual void OnGrabFunction()
    {
        isHeld = true;
    }

    public virtual void OnReleaseFunction()
    {
        isHeld = false;
    }

}
