using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerConfigMenuUIFlat : MonoBehaviour
{
    public UnityEngine.UI.Button ReadyButton;
    public UnityEngine.UI.Text ReadyText;

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BaseAccessor>();
        ReadyButton.onClick.RemoveAllListeners();
        ReadyButton.onClick.AddListener(delegate
        {
            if (accessor.IsOwner && NetworkManager.Singleton.IsHost)
            {
                accessor.PlayerConfigExit();
            } else
            {
                accessor.SetLockServerRpc(!accessor.Lock.Value);
            }
        });
    }
}
