using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class MatchConfigMenuUIFlat : MonoBehaviour
{
    public UnityEngine.UI.Button UseDefault;
    public UnityEngine.UI.Text JoinCode;

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BaseAccessor>();
        UseDefault.enabled = NetworkManager.Singleton.IsHost;
        UseDefault.onClick.RemoveAllListeners();
        UseDefault.onClick.AddListener(delegate
        {
            if (accessor.IsOwner) accessor.MatchConfigExit();
        });
    }
}
