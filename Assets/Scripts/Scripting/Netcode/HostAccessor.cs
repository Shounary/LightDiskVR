using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HostAccessor : CommonAccessor
{
    NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        m_GameStage.Value = GameStage.MatchConfig;
    }

    public override GameStage GetGameStage() { return m_GameStage.Value; }

    /// <summary>
    /// Checks if all clients connected to the current server are ready for a certain event
    /// </summary>
    /// <param name="lock_id">id for event, null means default event (in the current context) </param>
    /// <returns></returns>
    public bool ClientLock(string lock_id = null)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            IReadOnlyList<NetworkClient> clients = NetworkManager.Singleton.ConnectedClientsList;

            bool tally = true;
            foreach(NetworkClient client in clients)
            {
                ClientAccessor p = client.PlayerObject.GetComponent<ClientAccessor>();
                tally &= p.GetEventReadyState(lock_id);
                if (!tally) { break; }
            }

            return tally;
        }

        return false;
    }
}
