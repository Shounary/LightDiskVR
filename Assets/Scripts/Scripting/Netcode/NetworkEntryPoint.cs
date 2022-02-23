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

        DontDestroyOnLoad(gameObject);

        if (IsHost)
        {
            HostAccessor h_Accessor = gameObject.AddComponent<HostAccessor>();
            h_Accessor.EnterMatchConfig();

            accessor = h_Accessor;
        }
        else
        {
            ClientAccessor c_Accessor = gameObject.AddComponent<ClientAccessor>();
            
            // after match is already set, cannot join any more
            if (c_Accessor.GameStage <= GameStage.MatchConfig)
            {
                c_Accessor.EnterMatchConfigClientRPC();
            }
            else
            {
                Debug.Log("TOO LATE FOR JOINING GAME!");
                NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);

                Destroy(gameObject);
            }

            accessor = c_Accessor;
        }

        
    }
}
