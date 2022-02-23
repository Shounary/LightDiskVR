using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MatchConfigMenuUIFlat : MonoBehaviour
{
    public UnityEngine.UI.Button UseDefault;

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject.GetComponent<BaseAccessor>();
        UseDefault.enabled = accessor is HostAccessor;
        UseDefault.onClick.RemoveAllListeners();
        UseDefault.onClick.AddListener(delegate
        {
            try
            {
                HostAccessor hostAccessor = accessor as HostAccessor;
                hostAccessor.EnterPlayerConfig();
            }
            catch (UnityException e)
            {
                Debug.Log(e.StackTrace);
                Debug.Log("Cannot Click!");
            }
        });
    }
}
