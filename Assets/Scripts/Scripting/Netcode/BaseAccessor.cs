using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;

public abstract class BaseAccessor : NetworkBehaviour
{
    public NetworkObject PlayerObject
    {
        get { return NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject(); }
    }

    public abstract GameStage GameStage { get; }

    public abstract MatchConfig MatchConfig { get; }

    public abstract PlayerConfig PlayerConfig { get; }

    #region MATCHCONFIG
    protected Arena Arena;
    #endregion

    #region PLAYERCONFIG
    protected int SpawnPoint;
    #endregion
}
