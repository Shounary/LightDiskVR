using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HostAccessor : BaseAccessor
{
    NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        m_GameStage.Value = GameStage.MatchConfig;
    }

    public override GameStage GameStage => m_GameStage.Value;

    private MatchConfig m_MatchConfig;
    public override MatchConfig MatchConfig => m_MatchConfig;

    private PlayerConfig m_PlayerConfig;
    public override PlayerConfig PlayerConfig => m_PlayerConfig;

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

    public void EnterMatchConfig()
    {
        m_GameStage.Value = GameStage.MatchConfig;
        m_GameStage.SetDirty(true);

        m_MatchConfig = new MatchConfig();
        ChooseArena(MatchConfigFactory.Instance.Arena);
    }

    #region MATCHCONFIG
    public void ChooseArena(Arena arena)
    {
        m_MatchConfig.Arena = arena;
    }

    public void IncrementMaxTeam() { m_MatchConfig.MaxTeams++; }
    public void DecrementMaxTeam() { m_MatchConfig.MaxTeams--; }
    public void IncrementWinConditionIndex() { m_MatchConfig.WinConditionIndex++; }
    public void DecrementWinConditionIndex() { m_MatchConfig.WinConditionIndex--; }
    public void IncrementArena() { MatchConfigFactory.Instance.ArenaIndex++; }
    public void DecrementArena() { MatchConfigFactory.Instance.ArenaIndex--; }

    #endregion MATCHCONFIG
}
