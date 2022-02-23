using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;

public abstract class BaseAccessor : NetworkBehaviour
{
    protected NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>();

    public NetworkObject PlayerObject
    {
        get { return NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject(); }
    }

    public GameStage GameStage { get { return m_GameStage.Value; } }


    #region MATCHCONFIG
    protected NetworkVariable<int> ArenaID, MaxTeams, WinConditionIndex;
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

    public void EnterMatchConfig()
    {
        StartMenuUIFlat.Instance.StartMenu.SetActive(false);
        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(true);
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

    public void EnterPlayerConfig()
    {
        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);
        StartMenuUIFlat.Instance.PlayerConfigMenu.SetActive(true);
    }

    public void EnterMatch()
    {
        Debug.Log("Entering battle scene");

        StartMenuUIFlat.Instance.MatchConfigMenu.SetActive(false);
    }

    public void EnterResult()
    {
        Debug.Log("Going back go common scene");

        NetworkManager.Singleton.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(
                0
                ).name,
            UnityEngine.SceneManagement.LoadSceneMode.Single
            );
    }
}
