using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using Unity.Netcode.Samples;

public class NetworkWeapon : Weapon
{
    [SerializeField] NetworkObject NetworkObject;
    [SerializeField] ClientNetworkTransform WeaponCNT;
    [SerializeField] NetworkDisk NetworkDisk; // interface to all RPC calls

    public override void TriggerFunction(float additionalFactor, Transform targetTransform) { }

    private new void Start()
    {
        if (!NetworkObject.IsOwner)
        {
            //this.enabled = false;
            XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
            interactable.enabled = false;
        } else
        {
            base.Start();
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

    public override void OnReleaseFunction(int h)
    {
        isHeld = false;
        if(weaponInventory.weaponSelectScreens.Count > 0 && weaponInventory != null)
            weaponInventory.closeSelectUI(hand, false);
    }

    //when called, the weapon will be attracted to the target transfrom
    public new void AttractWeapon(float additionalFactor, Transform targetTransform) {

        Vector3 targetDirection = Vector3.Normalize(targetTransform.position - weaponRB.position);
        Vector3 initialDirection = Vector3.Normalize(weaponRB.velocity);
        float angle = Vector3.Angle(targetDirection, initialDirection);

        Vector3 normal = additionalFactor * stoppingFactorMultiplier * diskReturnForceMagnitude * Time.deltaTime * (-1) * Vector3.Magnitude(weaponRB.velocity) * Mathf.Abs(Mathf.Sin(Mathf.Abs(angle))) * initialDirection;
        Vector3 parallel = additionalFactor * diskReturnForceMagnitude * Time.deltaTime * targetDirection;

        if (angle > 5)
        {
            NetworkDisk.AttractWeaponServerRpc(normal);
        }

        NetworkDisk.AttractWeaponServerRpc(parallel);
    }

    //because weapon references are stored in the inventory script, actually destorying the weapon
    //gameobjects would be a pain to deal with. Instead, the weapon is disabled, and then can
    //be re-enabled later 
    public new void DestroyWeapon()
    {
        base.DestroyWeapon();
        NetworkDisk.DestroyWeaponServerRpc();
    }

    public override void MainButtonReleaseFunction() { }
}
