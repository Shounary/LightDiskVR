using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class DemoGrab : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            var match = FindObjectOfType<MatchConfigMenuUIFlat>().gameObject;
            match.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
            {
                GrabDisk();
            });
        }
    }

    private void GrabDisk()
    {
        // this section is purely for selecting disk
        // if you already know which disk to change owner to, just skip this section
        // and directly pass the known disk through the parameter
        // objectDiskPair is a tuple ( NetworkObject of the disk, NetworkDisk component of the disk)
        // get all disks in the scene, this part should be edited according to the comments
        var objDiskPair = 
            NetworkManager.SpawnManager.SpawnedObjectsList
            .Select(obj => (obj, obj.GetComponent<NetworkDisk>())) // tuple: NetworkObjct - NetworkDisk
            .Where(d => d.Item1 != null && d.Item2 != null
            ).FirstOrDefault();

        // another way is to pass the networkobject's global id, and then relevant components here
        // use this for grab with id
        // var obj = NetworkManager.SpawnManager.SpawnedObjects[objId];
        // var disk = obj.GetComponent<NetworkDisk>();


        if (!objDiskPair.Equals(default))
        {
            var obj = objDiskPair.Item1;
            var disk = objDiskPair.Item2;
            var oldOwnerId = obj.OwnerClientId;

            // technically there is no need to split between these two calls
            // this is just for demo purposes
            if (IsServer)
            {
                disk.ServerOwnsDisk();
            }
            else
            {
                disk.ClientOwnsDiskServerRpc(NetworkManager.LocalClientId);
            }

        } else
        {
            Debug.Log("no valid disk in scene");
        }
    }
}
