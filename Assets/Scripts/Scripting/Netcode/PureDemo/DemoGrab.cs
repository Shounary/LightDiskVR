using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class DemoGrab : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner)
        {
            var match = FindObjectOfType<MatchConfigMenuUIFlat>().gameObject;
            match.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
            {
                // get the synced id of the disk's network behaviour using NetworkBehaviour.NetworkObject.NetworkObjectId
                // then pass that hash to the RPC
                GrabDiskServerRpc(0);
            });
        }
    }

    [ServerRpc]
    private void GrabDiskServerRpc(ulong objId) // add a hash parameter here
    {
        // get all disks in the scene, this part should be edited according to the comments
        var objDiskPair = 
            NetworkManager.SpawnManager.SpawnedObjectsList
            .Select(obj => (obj, obj.GetComponent<NetworkDisk>())) // tuple: NetworkObjct - NetworkDisk
            .Where(d => d.Item1 != null && d.Item2 != null
            ).FirstOrDefault();

        // use this for grab with id
        // var obj = NetworkManager.SpawnManager.SpawnedObjects[objId];
        // var disk = obj.GetComponent<NetworkDisk>();


        if (!objDiskPair.Equals(default))
        {
            var obj = objDiskPair.Item1;
            var disk = objDiskPair.Item2;
            var oldOwnerId = obj.OwnerClientId;

            obj.ChangeOwnership(OwnerClientId);
            Debug.Log($"change ownership {oldOwnerId} => {obj.OwnerClientId}");

            // force call if already owner
            if (oldOwnerId == obj.OwnerClientId)
            {
                var owner = NetworkManager.ConnectedClients[oldOwnerId];
                disk.OnGainedOwnership();
            }

        } else
        {
            Debug.Log("no valid disk in scene");
        }
    }
}
