using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HostAccessor : BaseAccessor
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        m_GameStage.Value = GameStage.MatchConfig;
    }

    private MatchConfig m_MatchConfig;
    public override MatchConfig MatchConfig => m_MatchConfig;

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
                try
                {
                    ClientAccessor p = client.PlayerObject.GetComponent<ClientAccessor>();
                    tally &= p.GetEventReadyState(lock_id);
                    if (!tally) { break; }
                }
                catch { }
            }

            return tally;
        }

        return false;
    }


    #region MATCH_CONFIG
    public new void EnterMatchConfig()
    {
        base.EnterMatchConfig();
        Debug.Log(string.Format("Host {0} entered match config stage", NetworkManager.Singleton.LocalClientId));

        m_GameStage.Value = GameStage.MatchConfig;
        m_MatchConfig = new MatchConfig();

        ArenaID = new NetworkVariable<int>(m_MatchConfig.Arena.BuildIndex);
        MaxTeams = new NetworkVariable<int>(m_MatchConfig.MaxTeams);
        WinConditionIndex = new NetworkVariable<int>(m_MatchConfig.WinConditionIndex);
    }

    public void IncrementMaxTeam() { MaxTeams.Value = ++m_MatchConfig.MaxTeams;}
    public void DecrementMaxTeam() { MaxTeams.Value = --m_MatchConfig.MaxTeams; }
    public void IncrementWinConditionIndex() { WinConditionIndex.Value = ++m_MatchConfig.WinConditionIndex; }
    public void DecrementWinConditionIndex() { WinConditionIndex.Value = --m_MatchConfig.WinConditionIndex; }
    public void IncrementArena() { ArenaID.Value = ++MatchConfigFactory.Instance.ArenaIndex; }
    public void DecrementArena() { ArenaID.Value = --MatchConfigFactory.Instance.ArenaIndex; }

    #endregion

    #region PLAYER_CONFIG
    public new void EnterPlayerConfig()
    {
        Debug.Log("Exiting Match Config Stage as a Host");
        PrintMatchConfig();

        m_GameStage.Value = GameStage.PlayerConfig;
        m_MatchConfig = new MatchConfig();

        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log(string.Format("Attempt to let {0} proceed to Player Config stage", client.ClientId));
            client.PlayerObject.gameObject.GetComponent<ClientAccessor>().EnterPlayerConfigClientRPC();
        }

        base.EnterPlayerConfig();
    }
    #endregion

    public new void EnterMatch()
    {
        if (!ClientLock())
        {
            Debug.Log("Not all clients are ready!");
            return;
        }

        base.EnterMatch();
    }

    public new void EnterResult()
    {
        throw new System.NotImplementedException();
    }

    public void ClearLock(string lock_name)
    {
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            try
            {
                client.PlayerObject.GetComponent<ClientAccessor>().ClearLockClientRPC(lock_name);
            }
            catch { }
        }
    }
}
