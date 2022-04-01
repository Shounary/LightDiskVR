using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using System;

public class StartMenuUIFlat : Singleton<StartMenuUIFlat>
{
    [SerializeField]
    private Button startHostButton;
    [SerializeField]
    private Button startClientButton;
    [SerializeField]
    private Text joinCode;

    [SerializeField]
    GameObject GrabDemoDiskPrototype;

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
                try
                {
                    PreMatchManager.UnNetworkedXRRig.SetActive(false);
                }
                catch (Exception)
                {
                    Debug.LogError("No XR Rig Attached");
                }
                var disk = Instantiate(GrabDemoDiskPrototype, new Vector3(0f, 0.5f, 3f), Quaternion.identity);
                disk.GetComponent<NetworkObject>().Spawn(); //.SpawnWithOwnership

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
                try
                {
                    PreMatchManager.UnNetworkedXRRig.SetActive(false);
                }
                catch (Exception)
                {
                    Debug.LogError("No XR Rig Attached");
                }
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
