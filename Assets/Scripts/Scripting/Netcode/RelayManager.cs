using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;

// with references to https://github.com/dilmerv/UnityMultiplayerPlayground
public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance { get { return _instance; } }
    private static RelayManager _instance;
    public string JoinCode { get; protected set; }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        foreach(NetworkManager nm in
            GameObject
            .FindGameObjectsWithTag("DestroyOnRestart")
            .Where(go => go != gameObject)
            .Select(go => go.GetComponent<NetworkManager>())
            .Where(nm => nm != null))
        {
            nm.Shutdown();
            Destroy(nm.gameObject);
        }

        gameObject.GetComponent<NetworkManager>().SetSingleton();
    }

    // [SerializeField]
    // private string env = "production";

    [SerializeField]
    private int max_conn;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    public bool Relayable
    {
        get
        {
            return Transport?.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
        }
    }

    // with references to https://docs-multiplayer.unity3d.com/docs/relay/relay
    /// <summary>
    /// HostGame allocate a Relay server and returns needed data to host the game
    /// </summary>
    /// <param name="maxConn">The maximum number the Relay can have</param>
    /// <returns>A Task returning the needed hosting data</returns>
    public async Task<RelayHostData> HostGame()
    {
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        //Ask Unity Services to allocate a Relay server
        Allocation allocation = await Unity.Services.Relay.Relay.Instance.CreateAllocationAsync(Instance.max_conn);

        //Populate the hosting data
        RelayHostData data = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        //Retrieve the Relay join code for our clients to join our party
        data.JoinCode = await Unity.Services.Relay.Relay.Instance.GetJoinCodeAsync(data.AllocationID);
        JoinCode = data.JoinCode;

        return data;
    }

    /// <summary>
    /// Join a Relay server based on the JoinCode received from the Host or Server
    /// </summary>
    /// <param name="joinCode">The join code generated on the host or server</param>
    /// <returns>All the necessary data to connect</returns>
    public async Task<RelayJoinData> JoinGame(string joinCode)
    {
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        //Ask Unity Services for allocation data based on a join code
        JoinAllocation allocation = await Unity.Services.Relay.Relay.Instance.JoinAllocationAsync(joinCode);

        //Populate the joining data
        RelayJoinData data = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        // Debug.Log($"Client joined with {joinCode}");
        JoinCode = joinCode;

        return data;
    }
}
