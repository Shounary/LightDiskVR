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
            // uncomment this line to match id
            // && d.Item1.NetworkObjectId == objId
            ).FirstOrDefault();
        
        if (objDiskPair.Equals(
            default(
            (NetworkObject, NetworkDisk)
            )))
        {
            var obj = objDiskPair.Item1;
            var disk = objDiskPair.Item2;
            obj.ChangeOwnership(OwnerClientId);
        } else
        {
            Debug.Log("no valid disk in scene");
        }
    }
}
