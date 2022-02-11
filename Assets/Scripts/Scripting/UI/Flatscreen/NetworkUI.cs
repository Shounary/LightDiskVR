using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;
    [SerializeField]
    private Button startHostButton;
    [SerializeField]
    private Button startClientButton;
    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(delegate
        {
            Debug.Log(NetworkManager.Singleton.StartHost() ?
                "Host started..." : "Host could not be started...");
        });

        startServerButton.onClick.AddListener(delegate
        {
            Debug.Log(NetworkManager.Singleton.StartServer() ?
                "Server started..." : "Server could not be started...");
        });

        startClientButton.onClick.AddListener(delegate
        {
            Debug.Log(NetworkManager.Singleton.StartClient() ?
                "Client started..." : "Client could not be started...");
        });
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
    }


}
