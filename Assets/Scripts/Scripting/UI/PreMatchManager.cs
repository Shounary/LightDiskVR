using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PreMatchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StartMenuObj, m_MatchConfigMenuObj, m_PlayerConfigMenuObj, m_PersistentUIObj, m_UnNetworkedXRRig;
    public static PreMatchManager Instance { get; private set; }

    public static GameObject StartMenuObj => Instance.m_StartMenuObj;
    public static StartMenuUIFlat StartMenu => StartMenuObj.GetComponent<StartMenuUIFlat>();

    public static GameObject MatchConfigMenuObj => Instance.m_MatchConfigMenuObj;
    public static MatchConfigMenuUIFlat MatchConfigMenu => MatchConfigMenuObj.GetComponent<MatchConfigMenuUIFlat>();

    public static GameObject PlayerConfigMenuObj => Instance.m_PlayerConfigMenuObj;
    public static PlayerConfigMenuUIFlat PlayerConfigMenu => PlayerConfigMenuObj.GetComponent<PlayerConfigMenuUIFlat>();

    public static GameObject PersistentUIObj => Instance.m_PersistentUIObj;
    public static PreMatchPersistentUI Persistent => PersistentUIObj.GetComponent<PreMatchPersistentUI>();

    public static GameObject UnNetworkedXRRig => Instance.m_UnNetworkedXRRig;

    public static void ResumeStart()
    {
        Instance.ResumeStartEntry();
    }

    private void ResumeStartEntry()
    {
        SceneManager.activeSceneChanged += Disconnect;
        SceneManager.LoadScene(0);
    }

    private void Disconnect(Scene prev, Scene next)
    {
        SceneManager.activeSceneChanged -= Disconnect;

        foreach (NetworkObject obj in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
        {
            Destroy(obj.gameObject);
        }

        var nmHolder = NetworkManager.Singleton.gameObject;
        NetworkManager.Singleton.Shutdown();
        Destroy(nmHolder);
    }

    private void Awake()
    {
        Instance = this;
    }
}
