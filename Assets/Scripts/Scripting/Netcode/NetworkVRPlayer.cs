using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using System.Linq;
using Unity.Netcode.Samples;
using System;

public class NetworkVRPlayer : NetworkBehaviour
{
    //[SerializeField]
    //private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    public override void OnNetworkSpawn() => DisableClientInput();

    public NetworkVariable<int> health = new NetworkVariable<int>(100);

    private InputDevice targetDeviceRight;
    private InputDevice targetDeviceLeft;

    public void EnableClientInput()
    {

    }

    public void DisableClientInput()
    {
        if (IsClient && !IsOwner) {
            var clientActionControllers = GetComponentsInChildren<ActionBasedController>();
            var clientDeviceControllers = GetComponentsInChildren<XRBaseController>();
            //var directInteractors = GetComponentsInChildren<XRDirectInteractor>();

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
                DBcontroller.enableInputActions = false;
                DBcontroller.enableInputTracking = false;
            }
        }
    }

    private void Start()
    {
        List<InputDevice> inputDevicesRight = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevicesRight);

        targetDeviceRight = inputDevicesRight[0];

        List<InputDevice> inputDevicesLeft = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, inputDevicesLeft);

        targetDeviceLeft = inputDevicesLeft[0];
        
        transform.position = new Vector3(
            transform.position.x - OwnerClientId,
            transform.position.y,
            transform.position.z
            );
    }


    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if (IsClient && IsOwner)
        {
            var t0 = (float) NetworkManager.NetworkTimeSystem.ServerTime;
            var networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            var weaponRB = eventArgs.interactableObject.transform.GetComponent<Rigidbody>();
            weaponRB.isKinematic = false;
            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(
                    OwnerClientId,
                    networkObjectSelected,
                    weaponRB.velocity,
                    weaponRB.angularVelocity,
                    weaponRB.position,
                    weaponRB.rotation,
                    weaponRB.transform.localScale,
                    t0);
        }
    }

    public void OnReleaseGrabbableRight(SelectExitEventArgs eventArgs) {
        if (IsClient && IsOwner)
        {
            var t0 = (float) NetworkManager.NetworkTimeSystem.ServerTime;
            // get disk references
            var weaponTr = eventArgs.interactableObject.transform;
            var networkObjectSelected = weaponTr.GetComponent<NetworkObject>();
            var weaponRB = weaponTr.GetComponent<Rigidbody>();
            var weapon = weaponTr.GetComponent<Weapon>();
            var weaponCNT = weaponTr.GetComponent<ClientNetworkTransform>();

            // get disk kinematic state
            var wCompV = new Vector3();
            var wCompAV = new Vector3();

            // obtain controller velocity----
            if (targetDeviceRight.TryGetFeatureValue(CommonUsages.deviceVelocity, out var rightControllerVel))
                wCompV = rightControllerVel;

            if (targetDeviceRight.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out var rightControllerAngVel))
                wCompAV = rightControllerAngVel;
            //----
            
            // if (weaponRB.isKinematic)
            // {
            //     PrintDebugServerRpc(0);
            // } else
            // {
            //     PrintDebugServerRpc(1);
            // }

            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(
                    NetworkManager.ServerClientId,
                    networkObjectSelected,
                    wCompV,
                    wCompAV,
                    weaponRB.position,
                    weaponRB.rotation,
                    weaponTr.localScale,
                    t0);
        }
    }

    public void OnReleaseGrabbableLeft(SelectExitEventArgs eventArgs) {
        if (IsClient && IsOwner)
        {
            var t0 = (float) NetworkManager.NetworkTimeSystem.ServerTime;
            // get disk references
            var weaponTr = eventArgs.interactableObject.transform;
            var networkObjectSelected = weaponTr.GetComponent<NetworkObject>();
            var weaponRB = weaponTr.GetComponent<Rigidbody>();
            var weapon = weaponTr.GetComponent<Weapon>();
            var weaponCNT = weaponTr.GetComponent<ClientNetworkTransform>();

            // get disk kinematic state
            var wCompV = new Vector3();
            var wCompAV = new Vector3();

            // obtain controller velocity----
            if (targetDeviceLeft.TryGetFeatureValue(CommonUsages.deviceVelocity, out var leftControllerVel))
                wCompV = leftControllerVel;

            if (targetDeviceLeft.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out var leftControllerAngVel))
                wCompAV = leftControllerAngVel;
            //----
            
            // if (weaponRB.isKinematic)
            // {
            //     PrintDebugServerRpc(0);
            // } else
            // {
            //     PrintDebugServerRpc(1);
            // }

            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(
                    NetworkManager.ServerClientId,
                    networkObjectSelected,
                    wCompV,
                    wCompAV,
                    weaponRB.position,
                    weaponRB.rotation,
                    weaponTr.localScale,
                    t0);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestGrabbableOwnershipServerRpc(
        ulong newOwnerClientId, NetworkObjectReference networkObjectReference,
        Vector3 v, Vector3 av, Vector3 p, Quaternion r, Vector3 s, float t0)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
            var acc = NetworkManager.ConnectedClients[newOwnerClientId].PlayerObject.GetComponent<BaseAccessor>();
            acc.Player.RequestGrabbableOwnershipClientRpc(networkObjectReference, v, av, p, r, s, t0);
            Debug.Log($"ownership transfer to {acc.OwnerClientId} with {v} {av} {p} {r} {s}");
        }
    }

    [ClientRpc]
    public void RequestGrabbableOwnershipClientRpc(
        NetworkObjectReference networkObjectReference,
        Vector3 v, Vector3 av, Vector3 p, Quaternion r, Vector3 s, float t0)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            var nt = networkObject.GetComponent<ClientNetworkTransform>();
            var rb = networkObject.GetComponent<Rigidbody>();

            // nt.Teleport(p, r, s);
            // rb.velocity = v;
            // rb.angularVelocity = av;

            StartCoroutine(WaitUntilEditable(nt, () => {
                var prediction = 1.25f;
                var deltaT = (float) NetworkManager.NetworkTimeSystem.ServerTime - t0;
                nt.Teleport(p + prediction * deltaT * v, r * Quaternion.Euler(prediction * deltaT * av.x, prediction * deltaT * av.y, prediction * deltaT * av.z), s);
                rb.velocity = v;
                rb.angularVelocity = av;
            }));

            // record time delta between release and receive
            PrintServerTimeServerRpc(NetworkManager.NetworkTimeSystem.ServerTime - t0);
        }
    }

    IEnumerator WaitUntilEditable(ClientNetworkTransform nt, Action f) {
        while (IsClient && !nt.CanCommitToTransform) yield return null;
        if (IsClient) f();
    }

    Dictionary<int, string> logBook = new Dictionary<int, string>();

    [ServerRpc(RequireOwnership = false)]
    public void PrintDebugServerRpc(int id)
    {
        Debug.Log(logBook.ContainsKey(id) ? logBook[id] : $"log message {id}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrintServerTimeServerRpc(double t)
    {
        Debug.Log($"log server time (sec) {t}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrintLocalTimeServerRpc(double t)
    {
        Debug.Log($"log local time (sec) {t}");
    }

    [ServerRpc]
    public void SetHealthServerRpc(int health)
    {
        this.health.Value = health;
    }

}
