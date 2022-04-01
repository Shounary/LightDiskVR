using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class OnGrabWeaponAssigner : MonoBehaviour
{
    public int weaponsMapped;

    private void Start() {
        weaponsMapped = 0;
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        Weapon weapon = eventArgs.interactable.transform.GetComponent<Weapon>();
        WeaponInventory weaponInventory = eventArgs.interactor.transform.GetComponentInParent<WeaponInventory>();
        //Transform xrRigTransform = eventArgs.interactor.transform.GetComponentInParent<VRRig>().transform;
        if (weapon == null || weaponInventory == null)
            return;
        if (weaponsMapped < 2) {
            weaponInventory.weaponList[weaponsMapped] = weapon;
            weaponsMapped++;

            TeleportationDisk teleportationDisk = eventArgs.interactable.transform.GetComponent<TeleportationDisk>();
            if (teleportationDisk != null) {
                //teleportationDisk.playerTransform = xrRigTransform;
            }
        }
        if (weaponsMapped >= 2) {
            weaponInventory.activateWeapons();
        }
    }
}
