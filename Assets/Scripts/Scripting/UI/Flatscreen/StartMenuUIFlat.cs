using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class StartMenuUIFlat : Singleton<StartMenuUIFlat>
{
    [SerializeField]
    private Button startHostButton;
    [SerializeField]
    private Button startClientButton;
    [SerializeField]
    private TMP_InputField joinCode;

    private void Start()
    {
        startHostButton.onClick.AddListener(async delegate
        {
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
            } else
            {
                Debug.Log("Host could not be started");
            }
        });

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
    }
}
