using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;




public class Weapon : MonoBehaviour
{
    //Visuals
    public Color primaryColor;
    public Color accentColor;


    public float damage;
    public bool isSummonable;
    public List<string> storableLocations;
    public string player;
    //public Collider hurtBox; 
    public Hand hand;
    public Rigidbody weaponRB;


    
    public float diskReturnForceMagnitude = 5f;
    public float stoppingFactorMultiplier = 0.2f;

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

}
