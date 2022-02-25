using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerConfigMenuUIFlat : MonoBehaviour
{
    public UnityEngine.UI.Button UseDefault;

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BaseAccessor>();
        UseDefault.enabled = NetworkManager.Singleton.IsHost;
        UseDefault.onClick.RemoveAllListeners();
        UseDefault.onClick.AddListener(delegate
        {
            accessor.PlayerConfigExit();
        });
    }
}
