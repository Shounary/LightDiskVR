using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class OnGrabWeaponAssigner : MonoBehaviour
{
    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        Weapon weapon = eventArgs.interactable.transform.GetComponent<Weapon>();
        WeaponInventory weaponInventory = eventArgs.interactor.transform.GetComponentInParent<WeaponInventory>();
        Transform xrRigTransform = eventArgs.interactor.transform.GetComponentInParent<NetworkVRPlayer>().transform;
        if (weapon == null || weaponInventory == null)
            return;
        if (weaponInventory.weaponList.Count < 2) {
            weaponInventory.addWeapon(weapon);

            TeleportationDisk teleportationDisk = eventArgs.interactable.transform.GetComponent<TeleportationDisk>();
            if (teleportationDisk != null) {
                teleportationDisk.playerTransform = xrRigTransform;
            }
        }
        if (weaponInventory.weaponList.Count >= 2) {
            weaponInventory.activateWeapons();
        }
    }
}
