using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;

public abstract class CommonAccessor : NetworkBehaviour
{
    public NetworkObject PlayerObject
    {
        get { return NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject(); }
    }

    public abstract GameStage GetGameStage();
}
