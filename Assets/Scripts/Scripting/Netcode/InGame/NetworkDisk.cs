using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Samples;
using Unity.Netcode.Components;

public class NetworkDisk : NetworkBehaviour
{

    /////////////////////////////////////////////////////////////////////////////
    /// MAKE SURE NETWORK MANAGER HAS THE DISK IN THE LIST OF NETWORK OBJECTS ///
    /////////////////////////////////////////////////////////////////////////////

    [SerializeField] private Light CenterGlow;

    private void OnEnable()
    {
        if (IsClient && IsServer && !IsSpawned)
        {
            NetworkObject.Spawn();
        }
    }

    private void Awake()
    {
        GetComponent<ClientNetworkTransform>().enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<ClientNetworkTransform>().enabled = true;
        Logger.Log($"Disk {NetworkObjectId} is now spawned");
    }

    // called when LOCAL client gains ownership
    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();

        Logger.Log($"Disk {NetworkObjectId} is now owned by {OwnerClientId}");

        // since the client is always authorized to move the ClientNetworkTransform,
        // this movement will be synced
        if (OwnerClientId == 0)
        {
            transform.position += 0.5f * Vector3.right;
        }
        else
        {
            transform.position -= 0.5f * Vector3.left;
        }

        if (OwnerClientId == 0)
        {
            ServerOwnsDisk();
        }
        else if (OwnerClientId == 1)
        {
            ClientOwnsDiskServerRpc();
        }
    }

    void ServerOwnsDisk()
    {
        // other local variables need to be set on the client side by themselves
        // to notify this action, call client rpc
        PropagateServerOwnDiskClientRpc();
    }

    [ClientRpc]
    void PropagateServerOwnDiskClientRpc()
    {
        // light color is a non-network property
        CenterGlow.color = Color.red;
    }

    [ServerRpc]
    void ClientOwnsDiskServerRpc()
    {

        // even though the initiater of the server rpc is a client, this does not mean
        // it is doing a "client rpc". those changes only have effect on local.
        // instead, call server rpc first, and use that to call client rpc, this way
        // all clients are notified for the change
        PropagateClientOwnDiskClientRpc();
    }

    [ClientRpc]
    void PropagateClientOwnDiskClientRpc()
    {
        // light color is a non-network property
        CenterGlow.color = Color.green;
    }
}
