using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Samples;
using Unity.Netcode.Components;

[RequireComponent(typeof(ClientNetworkTransform))]
public class NetworkDisk : NetworkBehaviour
{

    /////////////////////////////////////////////////////////////////////////////
    /// MAKE SURE NETWORK MANAGER HAS THE DISK IN THE LIST OF NETWORK OBJECTS ///
    /////////////////////////////////////////////////////////////////////////////

    [SerializeField] private Light CenterGlow;

    private void Awake()
    {
        var t = GetComponent<ClientNetworkTransform>();

        // currently there are no reasons to sync scale
        t.SyncScaleX = false;
        t.SyncScaleY = false;
        t.SyncScaleZ = false;

        // until connect, do not activate
        t.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        var t = GetComponent<ClientNetworkTransform>();
        t.enabled = true;
        t.CanCommitToTransform = IsOwner && IsClient;
    }

    // called when LOCAL client gains ownership
    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        Logger.Log($"Local client gains ownership of disk {NetworkObjectId}");
    }

    public override void OnLostOwnership()
    {
        base.OnLostOwnership();
    }

    public void ServerOwnsDisk()
    {
        NetworkObject.ChangeOwnership(NetworkManager.LocalClientId);
        // other local variables need to be set on the client side by themselves
        // to notify this action, call client rpc
        PropagateServerOwnDiskClientRpc();
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)] // force secure delivery
    void PropagateServerOwnDiskClientRpc()
    {
        Logger.Log("Server Own Disk Propagated");
        // light color is a non-network property
        CenterGlow.color = Color.red;

        GetComponent<ClientNetworkTransform>().CanCommitToTransform = IsOwner;
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)] // force secure delivery
    public void ClientOwnsDiskServerRpc(ulong newOwner)
    {
        NetworkObject.ChangeOwnership(newOwner);
        // even though the initiater of the server rpc is a client, this does not mean
        // it is doing a "client rpc". those changes only have effect on local.
        // instead, call server rpc first, and use that to call client rpc, this way
        // all clients are notified for the change
        PropagateClientOwnDiskClientRpc();
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    void PropagateClientOwnDiskClientRpc()
    {

        Logger.Log("Client Own Disk Propagated");
        // light color is a non-network property
        CenterGlow.color = Color.green;

        GetComponent<ClientNetworkTransform>().CanCommitToTransform = IsOwner;
    }

    // this is just for demo
    // server moves the disk right
    // client moves the  disk left
    private void Update()
    {
        if (!IsOwner) return;

        if (IsOwnedByServer)
        {
            transform.position += Vector3.right * 0.01f;
        }
        else
        {
            transform.position += Vector3.left * 0.01f;
        }

        if (Mathf.Abs(transform.position.x) > 1)
        {
            var p = transform.position;
            p.x = Mathf.Sign(p.x);
            transform.position = p;
        }
    }
}
