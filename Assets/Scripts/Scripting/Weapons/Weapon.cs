using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



public class Weapon : MonoBehaviour
{

    public Sprite weaponImage;
    public string weaponName;
    public string weaponDescription;

    public int damage;
    public bool isSummonable;
    public string playerName;
    public Hand hand;
    public Rigidbody weaponRB;

    public Transform weaponTransform;
    public List<Transform> transforms = new List<Transform>();
    public bool isHeld;

    public bool isWeaponEnabled = true; //set this to false to disable the weapon (ie for tutorial or lobby)

    public float diskReturnForceMagnitude = 10f;
    public float stoppingFactorMultiplier = 0.2f;

    public Vector3 startLoc;
    public WeaponInventory weaponInventory;

    public bool FirstWeaponSummon;

    protected void Start() {
        if (startLoc == null)
            startLoc = this.gameObject.transform.position;
        if (weaponInventory != null)
            playerName = weaponInventory.playerName;
    }

    public virtual void TriggerFunction(float additionalFactor, Transform targetTransform) { }

    public void onAddToInventory(WeaponInventory wi)
    {
        weaponInventory = wi;
        weaponName = weaponName.Replace('_', '\n');
        playerName = wi.playerName;
    }

    public virtual void MainButtonFunction() {
        EnableWeapon(startLoc);
    }

    public virtual void SecondaryButtonFunction() { }


    public void setHand(int h)
    {
        setHand((Hand)h);
    }

    public void setHand(Hand h)
    {
        hand = h;
        if (transforms.Count > 0)
        {
            weaponTransform.position = transforms[(int)h].position;
            weaponTransform.rotation = transforms[(int)h].rotation;
        }
    }

    public virtual void OnGrabFunction(int h)
    {
        isHeld = true;
        setHand((Hand)h);
    }

    public virtual void GrabHeldFunction(float additionalFactor, Transform targetTransform) {
        if (isSummonable)
            AttractWeapon(additionalFactor, targetTransform);
    }

    public virtual void OnReleaseFunction(int h)
    {
        isHeld = false;
        if(weaponInventory.weaponSelectScreens.Count > 0 && weaponInventory != null)
            weaponInventory.closeSelectUI(hand, false);
    }


    //when called, the weapon will be attracted to the target transfrom
    public void AttractWeapon(float additionalFactor, Transform targetTransform) {
        if (!FirstWeaponSummon) {
            EnableWeapon(targetTransform.position);
            FirstWeaponSummon = true;
        }
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


    //because weapon references are stored in the inventory script, actually destorying the weapon
    //gameobjects would be a pain to deal with. Instead, the weapon is disabled, and then can
    //be re-enabled later 
    public void DestroyWeapon()
    {
        //play disintegration animation (implement later)
        this.gameObject.SetActive(false);
        weaponRB.velocity = Vector3.zero;
    }

    public virtual void MainButtonReleaseFunction() { }

    //when a weapon is disabled by being swapped with a different weapon
    public void DeactivateWeapon()
    {
        DestroyWeapon();
    }

    public void EnableWeapon() {
        EnableWeapon(startLoc);
    }

    //enabled the weapon and moves it to the given postiion
    public void EnableWeapon(Vector3 t, Quaternion? rot = null)
    {
        transform.position = t;
        if(rot.HasValue)
            transform.rotation = rot.Value;
        gameObject.SetActive(true);
        if (weaponInventory != null)
            playerName = weaponInventory.playerName;
        //play spawning animation (implement later)
    }
}
