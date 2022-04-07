using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



public class Weapon : MonoBehaviour
{
    //Visuals
    public Color primaryColor;
    public Color accentColor;

    public Sprite weaponImage;
    public string weaponName;
    public string weaponDescription;

    public int damage;
    public bool isSummonable;
    public string playerName;
    public Hand hand;
    public Rigidbody weaponRB;

    public Transform weaponTransform;
    //public Transform weaponTransformLeft;
    //public Transform weaponTransformRight;
    public List<Transform> transforms = new List<Transform>();

    public bool isHeld;

    public bool isWeaponEnabled = true; //set this to false to disable the weapon (ie for tutorial or lobby)

    public GameObject parentGameObject; //an empty gameobject with uniform scaling that serves as the default parent

    public float diskReturnForceMagnitude = 5f;
    public float stoppingFactorMultiplier = 0.2f;

    public Vector3 startLoc;
    public WeaponInventory weaponInventory;
    
    private void Awake() {
        //this.enabled = false;
    }

    private void Start() {
        parentGameObject = GameObject.FindGameObjectsWithTag("Empty Parent")[0];
        this.gameObject.transform.SetParent(parentGameObject.transform);
        if (startLoc == null)
            startLoc = this.gameObject.transform.position;
        if(weaponInventory != null)
           playerName = weaponInventory.playerName;
    }

    public virtual void TriggerFunction(float additionalFactor, Transform targetTransform)
    {
        if(isSummonable)
            AttractWeapon(additionalFactor, targetTransform);
    }

    public void onAddToInventory(WeaponInventory wi)
    {
        weaponInventory = wi;
        weaponName = weaponName.Replace('_','\n');
        playerName = wi.playerName;
    }

    public virtual void MainButtonFunction(){
        EnableWeapon(startLoc);
    }

    public virtual void SecondaryButtonFunction(){}


    public void setHand(int h)
    {
        setHand((Hand) h);
    }

    public void setHand(Hand h)
    {
        hand = h;
        if(transforms.Count > 0)
        {
            weaponTransform.position = transforms[(int) h].position;
            weaponTransform.rotation = transforms[(int) h].rotation;
        }
    }

    public virtual void OnGrabFunction(int h)
    {
        isHeld = true;
        setHand((Hand) h);
    }

    public virtual void OnReleaseFunction(int h)
    {
        isHeld = false;
        if(weaponInventory.weaponSelectScreens.Count > 0)
            weaponInventory.closeSelectUI(hand, false);
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


    //because weapon references are stored in the inventory script, actually destorying the weapon
    //gameobjects would be a pain to deal with. Instead, the weapon is disabled, and then can
    //be re-enabled later 
    public void DestroyWeapon()
    {
        //play disintegration animation (implement later)
        this.gameObject.SetActive(false);
        weaponRB.velocity = Vector3.zero;
    }


    //when a weapon is disabled by being swapped with a different weapon
    public void DeactivateWeapon()
    {
        DestroyWeapon();
    }

    public void EnableWeapon() {
        EnableWeapon(startLoc);
    }

    //enabled the weapon and moves it to the given postiion
    public void EnableWeapon(Vector3 t)
    {
        this.gameObject.transform.position = t;
        this.gameObject.SetActive(true);
        if(weaponInventory != null)
            playerName = weaponInventory.playerName;
        //play spawning animation (implement later)
    }

}
