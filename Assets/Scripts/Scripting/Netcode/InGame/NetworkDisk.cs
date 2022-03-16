using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Samples;
using Unity.Netcode.Components;

[RequireComponent(typeof(ClientNetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
public class NetworkDisk : NetworkBehaviour
{

    /////////////////////////////////////////////////////////////////////////////
    /// MAKE SURE NETWORK MANAGER HAS THE DISK IN THE LIST OF NETWORK OBJECTS ///
    /////////////////////////////////////////////////////////////////////////////

    [SerializeField] private Light CenterGlow;

    ClientNetworkTransform cnt;
    Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cnt = GetComponent<ClientNetworkTransform>();
        rb = GetComponent<Rigidbody>();
        cnt.enabled = true;
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)] // force secure delivery
    public void ClientOwnsDiskServerRpc(ulong newOwner)
    {
        NetworkObject.ChangeOwnership(newOwner);
        transform.parent = NetworkManager.ConnectedClients[newOwner].PlayerObject.transform;
        cnt.InLocalSpace = true;

        // even though the initiater of the server rpc is a client, this does not mean
        // it is doing a "client rpc". those changes only have effect on local.
        // instead, call server rpc first, and use that to call client rpc, this way
        // all clients are notified for the change
        PropagateClientOwnDiskClientRpc();
    }

    Color[] col_arr = {
        Color.blue,
        Color.cyan,
        Color.red,
        Color.green,
        Color.yellow
    };

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    void PropagateClientOwnDiskClientRpc()
    {
        Logger.Log($"Propagated to local client, cnt commit now set to {cnt.CanCommitToTransform}");
        // light color is a non-network property
        CenterGlow.color = col_arr[OwnerClientId % (ulong) col_arr.Length];
        rb.isKinematic = true;
    }

    // this is just for demo
    // server moves the disk right
    // client moves the  disk left
    private void Update()
    {
        if (!IsOwner) return;

        var p = transform.localPosition;

        if (OwnerClientId % 2 == 0)
        {
            p += Vector3.right * 0.01f;
        }
        else
        {
            p += Vector3.left * 0.01f;
        }

        p.x = Mathf.Sign(p.x);
        p.y = 0;
        // p.z = 1;

        transform.localPosition = p;
    }
}
