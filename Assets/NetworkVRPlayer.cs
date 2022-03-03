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


    public void EnableClientInput()
    {
        if (IsClient && IsOwner)
        {
            // var clientControllers = GetComponentsInChildren<ActionBasedController>();
            // foreach (var controller in clientControllers)
            // {
            //     controller.enableInputActions = true;
            //     controller.enableInputTracking = true;
            // }

            // var clientCamera = GetComponentInChildren<Camera>();
            // clientCamera.enabled = true;

            //var clientHead = clientCamera.gameObject.AddComponent<TrackedPoseDriver>();
            //clientHead.enabled = true;

        }
    }

    public void DisableClientInput()
    {
        if (IsClient && !IsOwner) {
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientHead.enabled = false;

            Debug.Log(clientControllers);
            foreach (var input in clientControllers)
            {
                input.enableInputActions = false;
                input.enableInputTracking = false;
                input.enabled = false;
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

    public override void OnNetworkSpawn() => DisableClientInput();
}
