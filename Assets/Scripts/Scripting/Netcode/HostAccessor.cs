using System.Collections;
using System.Linq;
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
        Debug.Log(string.Format("Host {0} entered match config", NetworkManager.Singleton.LocalClientId));

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
        Debug.Log("Exiting Player Config as a Host");
        PrintMatchConfig();

        m_GameStage.Value = GameStage.PlayerConfig;
        m_PlayerConfig = new PlayerConfig()
        {
            MatchConfig = m_MatchConfig
        };

        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log(string.Format("Attempt to let {0} proceed to Player Config", client.ClientId));
            try {
                client.PlayerObject.gameObject.GetComponent<ClientAccessor>().EnterPlayerConfigClientRPC();
            }
            catch { }
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

        IEnumerable<int> SpawnPointsList = NetworkManager.Singleton.ConnectedClientsList.Select((client) => {
            return client.PlayerObject.GetComponent<BaseAccessor>().PlayerConfig.SpawnPoint;
        });

        HashSet<int> SpawnPointsSet = new HashSet<int>(SpawnPointsList);

        if (SpawnPointsSet.Count() < SpawnPointsList.Count())
        {
            Debug.Log("Repeated spawn points!");
            foreach (int i in SpawnPointsList)
            {
                Debug.Log(string.Format("... {0}", i));
            }
            return;
        }

        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(MatchConfig.Arena.BuildIndex));

        Debug.Log(
            string.Format(
                "Host is starting the match in {0} at build index {1}",
                sceneName,
                MatchConfig.Arena.BuildIndex
            ));

        PrintPlayerConfig();

        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);

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
