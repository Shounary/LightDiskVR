using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public abstract class BaseAccessor : NetworkBehaviour
{
    protected NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>(GameStage.MatchConfig);

    public NetworkObject PlayerObject => NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

    public GameStage GameStage => m_GameStage.Value;


    #region MATCHCONFIG
    protected NetworkVariable<int> 
        ArenaID = new NetworkVariable<int>(0), 
        MaxTeams = new NetworkVariable<int>(1), 
        WinConditionIndex = new NetworkVariable<int>(0);
    public abstract MatchConfig MatchConfig { get; }
    #endregion

    #region PLAYERCONFIG
    protected int? SpawnPoint;
    protected PlayerConfig m_PlayerConfig;
    public PlayerConfig PlayerConfig => m_PlayerConfig;
    #endregion

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

    public virtual void EnterMatchConfig()
    {
        Debug.Log("Entering Match Config");
        StartMenuUIFlat.Instance.StartMenu.SetActive(false);
        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(true);

        BasePingServerRPC();
        BasePingClientRPC();
    }

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

    public void PrintPlayerConfig()
    {
        Debug.Log(
            string.Format(
                "... SpawnPoint: {0} located at {1}",
                m_PlayerConfig.SpawnPoint,
                m_PlayerConfig.SpawnPosition
            ));
    }

    public virtual void EnterPlayerConfig()
    {
        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);
        StartMenuUIFlat.Instance.PlayerConfigMenu.SetActive(true);
    }

    public virtual void EnterMatch()
    {
        Debug.Log("Entering battle scene");

        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);
    }

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

    public void RollCall(Action<ClientAccessor> action)
    {
        foreach (NetworkClient client in NetworkManager.ConnectedClientsList)
        {
            var acc = client.PlayerObject.GetComponent<BaseAccessor>();
            if (acc is ClientAccessor)
            {
                action(acc as ClientAccessor);
            }
        }
    }
}
