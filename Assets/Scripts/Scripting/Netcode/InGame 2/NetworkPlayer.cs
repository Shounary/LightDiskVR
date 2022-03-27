using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SpatialTracking;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    public override void OnNetworkSpawn()
    {
        DisableClientInput();
    }

    public void DisableClientInput()
    {
        if (IsClient && IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();
            var clientControllers = GetComponentInChildren<XRController>();

            clientCamera.enabled = false;
            //clientMoveProvider.EnableInputActions = false;
            clientHead.enabled = false;

            
        }
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
<<<<<<< Updated upstream
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y),
                transform.position.y, Random.Range(placementArea.x, placementArea.y));
=======
            transform.position = new Vector3(Random.RandomRange(placementArea.x, placementArea.y),
                transform.position.y, Random.RandomRange(placementArea.x, placementArea.y));
>>>>>>> Stashed changes
        }
    }
}
