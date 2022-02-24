using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientAccessor : BaseAccessor
{
    private Dictionary<string, bool> Lock;
    private bool DefaultLock;

    /// <summary>
    /// get ready/unready for event associated with key
    /// </summary>
    public void SetLock(bool ready, string key)
    {
        if (key == null)
        {
            DefaultLock = ready;
        }
        else
        {
            Lock.Add(key, ready);
        }
    }

    public bool GetEventReadyState(string key = null)
    {
        if (key == null)
        {
            return DefaultLock;
        }
        else
        {
            Lock.TryGetValue(key, out bool val);
            return val;
        }
    }

    public override MatchConfig MatchConfig { get { return s_MatchConfig; } }
    private MatchConfig s_MatchConfig;

    private void OnEnable()
    {
        s_MatchConfig = new MatchConfig();

        ArenaID.OnValueChanged += delegate
        {
            s_MatchConfig.Arena = MatchConfigFactory.Instance.GetArena(ArenaID.Value);
            // TODO: Update UI
        };

        MaxTeams.OnValueChanged += delegate
        {
            s_MatchConfig.MaxTeams = MaxTeams.Value;
            // TODO: Update UI
        };

        WinConditionIndex.OnValueChanged += delegate
        {
            s_MatchConfig.WinConditionIndex = MaxTeams.Value;
            // TODO: Update UI
        };

    }

    [ClientRpc]
    public void EnterPlayerConfigClientRPC()
    {
        Debug.Log("Exiting Player Config as a Client");

        m_PlayerConfig = new PlayerConfig()
        {
            MatchConfig = MatchConfig
        };
        m_PlayerConfig.SpawnPoint = 0;

        PrintMatchConfig();
        base.EnterPlayerConfig();
    }

    [ClientRpc]
    public void EnterMatchClientRPC()
    {
        PrintPlayerConfig();
        base.EnterMatch();
    }

    [ClientRpc]
    public void EnterResultClientRPC()
    {
        base.EnterResult();
    }

    [ClientRpc]
    public void ClearLockClientRPC(string lock_name)
    {
        SetLock(true, lock_name);
    }

}
