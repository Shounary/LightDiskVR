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
    private Button spawnObjButton;
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
                RelayManager.Instance.Transport.SetRelayServerData(
                    hostData.IPv4Address,
                    hostData.Port,
                    hostData.AllocationIDBytes,
                    hostData.Key,
                    hostData.ConnectionData
                    );
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
            RelayJoinData joinData = new RelayJoinData();

            if (RelayManager.Instance.Relayable && joinCode.text != null)
            {
                joinData = await RelayManager.Instance.JoinGame(joinCode.text);

                RelayManager.Instance.Transport.SetClientRelayData(
                    joinData.IPv4Address,
                    joinData.Port,
                    joinData.AllocationIDBytes,
                    joinData.Key,
                    joinData.ConnectionData,
                    joinData.HostConnectionData
                    );
            }

            var successful = NetworkManager.Singleton.StartClient();

            if (successful)
            {
                Debug.Log("Client connected to " + joinData.IPv4Address + ":" + joinData.Port + " under join code " + joinCode.text);

                joinCode.text = null;
            }
            else
            {
                Debug.Log("Client could not be started");
            }
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
