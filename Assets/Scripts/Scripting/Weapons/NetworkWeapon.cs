using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.Netcode;
using Unity.Netcode.Samples;

public class NetworkWeapon : Weapon
{
    [SerializeField] NetworkObject NetworkObject;
    [SerializeField] ClientNetworkTransform WeaponCNT;

    public override void TriggerFunction(float additionalFactor, Transform targetTransform) { }

    private new void Start()
    {
        base.Start();
        if (!NetworkObject.IsOwner)
        {
            this.enabled = false;
        }
    }

    public new void onAddToInventory(WeaponInventory wi)
    {
        weaponInventory = wi;
        weaponName = weaponName.Replace('_', '\n');
        playerName = wi.playerName;
    }

    public override void MainButtonFunction() {
        EnableWeapon(startLoc);
    }

    public override void SecondaryButtonFunction() { }

    public new void setHand(int h)
    {
        setHand((Hand)h);
    }

    public new void setHand(Hand h)
    {
        hand = h;
        if (transforms.Count > 0)
        {
            weaponTransform.position = transforms[(int)h].position;
            weaponTransform.rotation = transforms[(int)h].rotation;
        }
    }

    public override void OnGrabFunction(int h)
    {
        isHeld = true;
        setHand((Hand)h);
    }

    public override void GrabHeldFunction(float additionalFactor, Transform targetTransform) {
        if (isSummonable)
            AttractWeapon(additionalFactor, targetTransform);
    }

    public override void OnReleaseFunction(int h)
    {
        isHeld = false;
        if(weaponInventory.weaponSelectScreens.Count > 0 && weaponInventory != null)
            weaponInventory.closeSelectUI(hand, false);
    }


    //when called, the weapon will be attracted to the target transfrom
    public new void AttractWeapon(float additionalFactor, Transform targetTransform) {
        if (!FirstWeaponSummon)
        {
            EnableWeapon(targetTransform.position);
            FirstWeaponSummon = true;
        }
        Vector3 targetDirection = Vector3.Normalize(targetTransform.position - weaponRB.position);
        Vector3 initialDirection = Vector3.Normalize(weaponRB.velocity);
        float angle = Vector3.Angle(targetDirection, initialDirection);

        Vector3 normal = additionalFactor * stoppingFactorMultiplier * diskReturnForceMagnitude * Time.deltaTime * (-1) * Vector3.Magnitude(weaponRB.velocity) * Mathf.Abs(Mathf.Sin(Mathf.Abs(angle))) * initialDirection;
        Vector3 parallel = additionalFactor * diskReturnForceMagnitude * Time.deltaTime * targetDirection;

        if (angle > 5)
        {
            AttractWeaponServerRpc(normal);
        }

        AttractWeaponServerRpc(parallel);
    }

    [ServerRpc]
    public void AttractWeaponServerRpc(Vector3 f)
    {
        weaponRB.AddForce(f, ForceMode.VelocityChange);
    }

    //because weapon references are stored in the inventory script, actually destorying the weapon
    //gameobjects would be a pain to deal with. Instead, the weapon is disabled, and then can
    //be re-enabled later 
    public new void DestroyWeapon()
    {
        //play disintegration animation (implement later)
        this.gameObject.SetActive(false);
        weaponRB.velocity = Vector3.zero;
        DestroyWeaponServerRpc();
    }

    [ServerRpc]
    void DestroyWeaponServerRpc()
    {
        NetworkObject.Despawn(true);
    }

    public override void MainButtonReleaseFunction() { }

    //when a weapon is disabled by being swapped with a different weapon
    public new void DeactivateWeapon()
    {
        DestroyWeapon();
    }

    //enabled the weapon and moves it to the given postiion
    public new void EnableWeapon(Vector3 t)
    {
        this.gameObject.transform.position = t;
        this.gameObject.SetActive(true);
        if (weaponInventory != null)
            playerName = weaponInventory.playerName;
        //play spawning animation (implement later)
    }
}
