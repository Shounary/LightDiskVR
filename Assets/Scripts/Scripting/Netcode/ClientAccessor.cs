using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientAccessor : BaseAccessor
{
    private Dictionary<string, bool> Lock;
    private bool DefaultLock;

    public override GameStage GameStage {
        get {
            return GameStage.MatchConfig;
        }
    }
    

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

    // TODO: implement
    public override MatchConfig MatchConfig { get { throw new System.NotImplementedException(); return null; } }
    public override PlayerConfig PlayerConfig { get { throw new System.NotImplementedException(); return null; } }
}
