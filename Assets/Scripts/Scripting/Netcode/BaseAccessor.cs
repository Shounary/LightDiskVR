using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Linq;

public class BaseAccessor : NetworkBehaviour
{
    protected NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>(GameStage.MatchConfig);

    public NetworkObject PlayerObject => NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

    public GameStage GameStage => m_GameStage.Value;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        MatchConfigEntry();
    }

    #region MATCH_CONFIG
    protected NetworkVariable<int> 
        ArenaID = new NetworkVariable<int>(0), 
        MaxTeams = new NetworkVariable<int>(1), 
        WinConditionIndex = new NetworkVariable<int>(0);
    public MatchConfig MatchConfig { get; protected set; }

    public void MatchConfigEntry()
    {
        Debug.Log("Entering Match Config");
        StartMenuUIFlat.Instance.StartMenu.SetActive(false);
        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(true);

        MatchConfigFactory.Instance.ArenaIndex = 0;
        MatchConfig = new MatchConfig(MatchConfigFactory.Instance.Arena);

        if (IsHost)
        {
            MatchConfigServerPath();
        }
        MatchConfigClientPath();
    }

    public void MatchConfigServerPath()
    {
        Debug.Log(string.Format("Host {0} entered match config", NetworkManager.Singleton.LocalClientId));
        m_GameStage.Value = GameStage.MatchConfig;

        ArenaID.Value = MatchConfig.Arena.BuildIndex;
        MaxTeams.Value = MatchConfig.MaxTeams;
        WinConditionIndex.Value = MatchConfig.WinConditionIndex;
    }

    public void MatchConfigClientPath()
    {
        Debug.Log(string.Format("Client {0} entered match config", NetworkManager.Singleton.LocalClientId));

        ArenaID.OnValueChanged += delegate
        {
            MatchConfig.Arena = MatchConfigFactory.Instance.GetArena(ArenaID.Value);
            // TODO: Update UI
        };

        MaxTeams.OnValueChanged += delegate
        {
            MatchConfig.MaxTeams = MaxTeams.Value;
            // TODO: Update UI
        };

        WinConditionIndex.OnValueChanged += delegate
        {
            MatchConfig.WinConditionIndex = MaxTeams.Value;
            // TODO: Update UI
        };
    }

    public void MatchConfigExit()
    {
        RollCall(c => c.PlayerConfigEnterClientRPC());
    }

    public void PrintMatchConfig()
    {
        Debug.Log(
            string.Format(
                "... Arena: {0}, MaxTeams {1}, WinCondition: {2}",
                MatchConfig.Arena.name,
                MatchConfig.MaxTeams,
                MatchConfig.WinCondition.Item1)
            );
    }
    #endregion

    #region PLAYER_CONFIG
    public PlayerConfig PlayerConfig { get; protected set; }

    public void PrintPlayerConfig()
    {
        Debug.Log(
            string.Format(
                "... SpawnPoint: {0} located at {1}",
                PlayerConfig.SpawnPoint,
                PlayerConfig.SpawnPosition
            ));
    }

    [ClientRpc]
    public void PlayerConfigEnterClientRPC()
    {
        PlayerConfig = new PlayerConfig(MatchConfig);
        PlayerConfig.SpawnPoint = (int) NetworkManager.LocalClientId;

        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);
        StartMenuUIFlat.Instance.PlayerConfigMenu.SetActive(true);
    }

    public void PlayerConfigExit()
    {
        RollCall(c => c.EnterMatchClientRPC());
        EnterMatchServerRpc();
    }
    #endregion

    #region ENTER_MATCH
    [ClientRpc]
    public void EnterMatchClientRPC()
    {
        EnterMatch();
    }

    [ServerRpc]
    public void EnterMatchServerRpc()
    {
        if (!ClientLock(null))
        {
            Debug.Log("Not all clients are ready!");
            return;
        }

        IEnumerable<int> SpawnPointsList = NetworkManager.Singleton.ConnectedClientsList
            .Select((client) => client.PlayerObject.GetComponent<BaseAccessor>().PlayerConfig.SpawnPoint);

        int j = 0;
        foreach (int i in SpawnPointsList)
        {
            Debug.Log(string.Format("player {0} chose spawn point {1}", j++, i));
        }

        HashSet<int> SpawnPointsSet = new HashSet<int>(SpawnPointsList);

        if (SpawnPointsSet.Count() < SpawnPointsList.Count())
        {
            Debug.Log("Repeated spawn points!");
            return;
        }

        ClearLock(null);

        EnterMatch();
    }

    public void EnterMatch()
    {
        PrintPlayerConfig();

        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);

        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(MatchConfig.Arena.BuildIndex));
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        Debug.Log(
            string.Format(
                "Entering the match in {0} at build index {1}",
                sceneName,
                MatchConfig.Arena.BuildIndex
            ));
    }
    #endregion

    #region ENTER_RESULT
    public virtual void EnterResult()
    {
        Debug.Log("Going back go common scene");

        NetworkManager.Singleton.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(
                0
                ).name,
            UnityEngine.SceneManagement.LoadSceneMode.Single
            );
    }

    #endregion


    #region RPC_COMMONS
    [ServerRpc]
    public void BasePingServerRPC()
    {
        Debug.Log("Ping server");
    }
    
    [ClientRpc]
    public void BasePingClientRPC()
    {
        Debug.Log("Ping client");
    }

    public void RollCall(Action<BaseAccessor> action)
    {
        foreach (
            BaseAccessor acc in NetworkManager.ConnectedClientsList
            .Select(c => c.PlayerObject.GetComponent<BaseAccessor>())
            .Where(a => a != null))
            action(acc);
    }
    #endregion

    #region LOCK

    private Dictionary<string, bool> Lock;
    private bool DefaultLock = false;

    bool ClientLock(string lock_id) =>
        NetworkManager.ConnectedClientsList.Select(c => lock_id == null ? 
            c.PlayerObject.GetComponent<BaseAccessor>().DefaultLock 
            : c.PlayerObject.GetComponent<BaseAccessor>().Lock[lock_id])
        .Contains(false);

    void SetLock(string lock_id, bool val)
    {
        if (lock_id == null) DefaultLock = val;
        else Lock[lock_id] = val;
    }

    void ClearLock(string lock_id)
    {
        foreach (BaseAccessor acc in NetworkManager.ConnectedClientsList.Select(client => client.PlayerObject.GetComponent<BaseAccessor>()))
            acc.SetLock(lock_id, false);
    }
    #endregion
}
