using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BaseAccessor : NetworkBehaviour
{
    protected NetworkVariable<GameStage> m_GameStage = new NetworkVariable<GameStage>(GameStage.MatchConfig);

    public NetworkObject PlayerObject => NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

    public GameStage GameStage => m_GameStage.Value;

    protected NetworkVariable<NetworkString> m_PlayerListText = new NetworkVariable<NetworkString>("Player List");

    public string PlayerListText => m_PlayerListText.Value;

    public override void OnNetworkSpawn()
    {
        UpdatePlayerListTextServerRPC();
        MatchConfigEntry();

        DontDestroyOnLoad(gameObject);
        NetworkManager.SceneManager.OnLoadComplete += PersistGameObject;

        Lock.OnValueChanged = (prev, next) =>
        {
            UpdatePlayerListTextServerRPC();
        };
    }

    [ServerRpc(RequireOwnership=false)]
    private void UpdatePlayerListTextServerRPC()
    {
        var thisObj = GetComponent<NetworkObject>();
        m_PlayerListText.Value = string.Concat(
            NetworkManager.Singleton.ConnectedClientsList
            .Select(c =>
            {
                BaseAccessor acc = c.PlayerObject.GetComponent<BaseAccessor>();
                string str = "";
                /*if (c.ClientId == thisObj.OwnerClientId)
                {
                    str += "<B>";
                }*/
                str += $"P{c.ClientId}";
                if (acc.OwnerClientId == NetworkManager.LocalClientId)
                {
                    str += " (Host)";
                }
                else
                {
                    if (acc.Lock.Value)
                    {
                        str += " Ready";
                    }
                    else
                    {
                        str += " Not Ready";
                    }
                }
                /*if (c.ClientId == thisObj.OwnerClientId)
                {
                    str += "</B>";
                }*/
                str += "\n";

                return str;
            }
            ).DefaultIfEmpty(""));
    }

    #region MATCH_CONFIG
    protected NetworkVariable<int> 
        ArenaID = new NetworkVariable<int>(0), 
        MaxTeams = new NetworkVariable<int>(1), 
        WinConditionIndex = new NetworkVariable<int>(0);
    public MatchConfig MatchConfig { get; protected set; }

    public void MatchConfigEntry()
    {
        PreMatchManager.StartMenuObj.SetActive(false);
        PreMatchManager.MatchConfigMenuObj.SetActive(true);
        PreMatchManager.PersistentUIObj.SetActive(true);
        PreMatchManager.Persistent.Accessor = this;
        MatchConfigFactory.Instance.ArenaIndex = 0;
        MatchConfig = new MatchConfig(MatchConfigFactory.Instance.Arena);

        // FIXME:
        // currently the delay is added because network variables are not initialized upon networkspawn
        this.DelayLaunch(delegate
        {
            ArenaID.OnValueChanged = null;
            MaxTeams.OnValueChanged = null;
            WinConditionIndex.OnValueChanged = null;

            MatchConfigServerRpc();
            MatchConfigClientPath();
        }, 0.1f);
    }

    [ServerRpc(RequireOwnership =false)]
    public void MatchConfigServerRpc()
    {
        Debug.Log($"Host {NetworkManager.Singleton.LocalClientId} entered match config");
        m_GameStage.Value = GameStage.MatchConfig;

        ArenaID.Value = MatchConfig.Arena.BuildIndex;
        MaxTeams.Value = MatchConfig.MaxTeams;
        WinConditionIndex.Value = MatchConfig.WinConditionIndex;

        //ArenaID.OnValueChanged = (prev, next) => { RollCall(acc => acc.ArenaID.Value = next); };
        //MaxTeams.OnValueChanged = (prev, next) => { RollCall(acc => acc.MaxTeams.Value = next); };
        //WinConditionIndex.OnValueChanged = (prev, next) => { RollCall(acc => acc.WinConditionIndex.Value = next); };

        SetLockServerRpc(false);
    }

    public void MatchConfigClientPath()
    {
        Debug.Log($"Client {NetworkManager.Singleton.LocalClientId} entered match config");

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

        PreMatchManager.MatchConfigMenu.JoinCode.text = RelayManager.Instance.JoinCode;
    }

    public void MatchConfigExit()
    {
        RollCall(acc => { acc.PlayerConfigEnterClientRPC(); acc.PlayerConfigEnterServerRPC(); });
    }

    public void PrintMatchConfig()
    {
        Debug.Log(
            $"... Arena: {MatchConfig.Arena.name}, MaxTeams {MatchConfig.MaxTeams}, WinCondition: {MatchConfig.WinCondition.Item1}"
            );
    }
    #endregion

    #region PLAYER_CONFIG
    public PlayerConfig PlayerConfig { get; protected set; }
    NetworkVariable<int> SpawnPoint = new NetworkVariable<int>(0);

    public void PrintPlayerConfig()
    {
        Debug.Log(
            $"... SpawnPoint: {PlayerConfig.SpawnPoint} located at {PlayerConfig.SpawnPosition}"
            );
    }

    [ClientRpc]
    public void PlayerConfigEnterClientRPC()
    {
        PlayerConfig = new PlayerConfig(MatchConfig);
        PreMatchManager.MatchConfigMenuObj.SetActive(false);
        PreMatchManager.PlayerConfigMenuObj.SetActive(true);
    }

    [ServerRpc(RequireOwnership =false)]
    public void PlayerConfigEnterServerRPC()
    {
        m_GameStage.Value = GameStage.PlayerConfig;
        SpawnPoint.OnValueChanged = (prev, next) => {
            PlayerConfig.SpawnPoint = SpawnPoint.Value;
        };
        SpawnPoint.Value = MathUtils.Mod(Convert.ToInt32(OwnerClientId), MatchConfig.Arena.SpawnPoints.Length);
        Debug.Log(SpawnPoint.Value + " " + MatchConfig.Arena.SpawnPoints.Length + " " + Convert.ToInt32(NetworkManager.LocalClientId) + " " + NetworkManager.LocalClientId);
    }

    public void PlayerConfigExit()
    {
        EnterMatchServerRpc();
    }
    #endregion

    #region ENTER_MATCH
    [ClientRpc]
    public void EnterMatchClientRPC()
    {
        SceneManager.activeSceneChanged += EnterMatchSceneClientPath;
    }

    [ServerRpc]
    public void EnterMatchServerRpc()
    {
        (bool, int, int) lpf = GetLock();
        bool lockState = lpf.Item1;
        int tNum = lpf.Item2, fNum = lpf.Item3;

        if (!lockState)
        {
            Debug.Log($"{tNum} / {tNum + fNum}");

            return;
        }

        IEnumerable<int> SpawnPointsList = NetworkManager.Singleton.ConnectedClientsList
            .Select((client) => client.PlayerObject.GetComponent<BaseAccessor>().SpawnPoint.Value);

        int j = 0;
        foreach (int i in SpawnPointsList)
        {
            Debug.Log($"player {j++} chose spawn point {i}");
        }

        HashSet<int> SpawnPointsSet = new HashSet<int>(SpawnPointsList);

        if (SpawnPointsSet.Count() < SpawnPointsList.Count())
        {
            Debug.Log("Repeated spawn points!");
            return;
        }

        RollCall(c => c.EnterMatchClientRPC());

        EnterMatch();
    }

    public void EnterMatch()
    {
        PrintPlayerConfig();

        if (IsServer && IsOwner){
            SceneManager.activeSceneChanged += EnterMatchSceneServerPath;
        }
        
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(MatchConfig.Arena.BuildIndex));
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        Debug.Log($"Entering the match in {sceneName} at build index {MatchConfig.Arena.BuildIndex}");
    }

    public void EnterMatchSceneServerPath(Scene prev, Scene next)
    {
        RollCall(acc => acc.m_GameStage.Value = GameStage.DuringMatch);
        //SpawnXRRigs();
        SceneManager.activeSceneChanged -= EnterMatchSceneServerPath;
    }

    public void EnterMatchSceneClientPath(Scene prev, Scene next)
    {
        SceneManager.activeSceneChanged -= EnterMatchSceneClientPath;
    }
    #endregion

    #region ENTER_RESULT
    public virtual void EnterResult()
    {
        throw new NotImplementedException();
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

    public void RollCall(Action<BaseAccessor> action, bool skipHost = false)
    {
        foreach (
            BaseAccessor acc in NetworkManager.ConnectedClientsList
            .Select(c => c.PlayerObject.GetComponent<BaseAccessor>())
            .Where(a => a != null)
            .Where(a => !skipHost || a.OwnerClientId != NetworkManager.ServerClientId))
            action(acc);
    }

    private void PersistGameObject(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        DontDestroyOnLoad(gameObject);
    }

    [ServerRpc(RequireOwnership =false)]
    public void DespawnServerRpc()
    {
        NetworkObject.Despawn();
        Destroy(gameObject);
    }
    #endregion

    #region XR_RIG_CONTROL

    public NetworkVRPlayer Player;
    [SerializeField]
    private PauseMenu PauseMenu;

    private void Update()
    {
        if (IsSpawned && IsOwner)
        {
            CheckPause();
        }
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            return;
        }
    }

    public void ActivatePause() {
        PauseMenu.gameObject.SetActive(true);
    }

    public void TogglePause()
    {
        PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeSelf);
    }
    #endregion

    #region LOCK
    [NonSerialized]
    public NetworkVariable<bool> Lock = new NetworkVariable<bool>(true);

    public (bool, int, int) GetLock()
    {
        bool valid = true;
        int T = 0, F = 0;
        RollCall(acc => {
            Debug.Log($"Lock {acc.OwnerClientId} -> {acc.Lock.Value}");
            valid &= acc.Lock.Value;
            if (acc.Lock.Value) T++;
            else F++;
            }, true);
        return (valid, T, F);
    }

    [ServerRpc(RequireOwnership =false)]
    public void SetLockServerRpc(bool val)
    {
        Lock.Value = val;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetAllLocksServerRpc(bool val, bool skipHost = true)
    {
        RollCall(acc => acc.SetLockServerRpc(val));
    }
    #endregion
}
