using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkDisk : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Logger.Log($"Disk {NetworkObjectId} is now spawned");
    }

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();

        Logger.Log($"Disk {NetworkObjectId} is now owned by {OwnerClientId}");
    }
}
