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
    [SerializeField]
    private TMP_InputField joinCode;
    [SerializeField]
    private Button joinCodeButton;

    private string sharedJoinCode;

    private void Awake()
    {
        
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(async delegate
        {
            sharedJoinCode = null;
            RelayHostData hostData = new RelayHostData() { 
                JoinCode = null
            };
            if (RelayManager.Instance.Relayable && joinCode.text != null)
            {
                hostData = await RelayManager.Instance.HostGame();
            }

            var successful = NetworkManager.Singleton.StartHost();

            if (successful)
            {
                Debug.Log("Host started at " + hostData.IPv4Address + ":" + hostData.Port + " with join code " + hostData.JoinCode);
                sharedJoinCode = hostData.JoinCode;
            } else
            {
                Debug.Log("Host could not be started");
            }
        });

        //startServerButton.onClick.AddListener(delegate
        //{
        //    Debug.Log(NetworkManager.Singleton.StartServer() ?
        //        "Server started..." : "Server could not be started...");
        //});

        startClientButton.onClick.AddListener(async delegate
        {
            if (RelayManager.Instance.Relayable && joinCode.text != null)
                await RelayManager.Instance.JoinGame(joinCode.text);
            Debug.Log(NetworkManager.Singleton.StartClient() ?
                "Client started..." : "Client could not be started...");
        });

        joinCodeButton.onClick.AddListener(delegate
        {
            if (sharedJoinCode != null)
            {
                SetJoinButtonText(sharedJoinCode);
            }
        });
    }

    IEnumerator ResumeText(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SetJoinButtonText("Get Join Code");
    }

    private void SetJoinButtonText(string text)
    {
        joinCodeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
    }


}
