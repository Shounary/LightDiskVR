using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class NetworkVRPlayer : NetworkBehaviour
{
    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    public override void OnNetworkSpawn() => DisableClientInput();
    

    public void EnableClientInput()
    {

    }

    public void DisableClientInput()
    {
        if (IsClient && !IsOwner) {
            var clientActionControllers = GetComponentsInChildren<ActionBasedController>();
            var clientDeviceControllers = GetComponentsInChildren<XRBaseController>();
            var directInteractors = GetComponentsInChildren<XRDirectInteractor>();

            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientHead.enabled = false;

            foreach (var input in clientActionControllers)
            {
                input.enableInputActions = false;
                input.enableInputTracking = false;
                input.enabled = false;
            }

            foreach (var DBcontroller in clientDeviceControllers) {
                DBcontroller.enabled = false;
            }

            foreach (var directIteractor in directInteractors) {
                directIteractor.enabled = false;
            }
        }
    }

    private void Start()
    {
        // TODO: change to spawn point, and also depending on scene
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y),
                transform.position.y, Random.Range(placementArea.x, placementArea.y));
        }
    }


    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if (IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactable.transform.GetComponent<NetworkObject>();
            //Rigidbody weaponRB = eventArgs.interactable.transform.GetComponent<Rigidbody>();
            //weaponRB.isKinematic = false;
            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
        }
    }

    public void OnReleaseGrabbable(SelectExitEventArgs eventArgs) {
        if (IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactable.transform.GetComponent<NetworkObject>();
            //Rigidbody weaponRB = eventArgs.interactable.transform.GetComponent<Rigidbody>();
            //weaponRB.isKinematic = false;
            //if (networkObjectSelected != null)
            //    RequestGrabbableOwnershipServerRpc(NetworkManager.ServerClientId, networkObjectSelected);
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
    }
}
