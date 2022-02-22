using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking;
using UnityEngine;

public class NetworkEntryPoint : NetworkBehaviour
{
    BaseAccessor accessor;

    private void Start()
    {
        Debug.Log(
            string.Format(
                "IsHost={0} IsClient={1} IsOwner={2}",
                IsHost, IsClient, IsOwner)
            );

        if (IsHost)
        {
            HostAccessor c_Accessor = gameObject.AddComponent<HostAccessor>();
            accessor = c_Accessor;

            // set game stage
            c_Accessor.EnterMatchConfig();
        }
        else
        {
            accessor = gameObject.AddComponent<ClientAccessor>();
        }

        Debug.Log(accessor.GameStage);
    }
}
