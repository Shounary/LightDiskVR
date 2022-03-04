using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using FMODUnity;

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

    public static List<GameObject> TogglablePanels => new List<GameObject>{
        Instance.m_PlayerConfigMenuObj,
        Instance.m_MatchConfigMenuObj,
        Instance.m_StartMenuObj
    };

    [SerializeField]
    private EventReference panelSound;

    private void Start()
    {
        UpdatePanelDisplay(null);
    }

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

        UpdatePanelDisplay(null);

        var nmHolder = NetworkManager.Singleton.gameObject;
        NetworkManager.Singleton.Shutdown();
        Destroy(nmHolder);
    }

    public void UpdatePanelDisplay(GameStage? stage)
    {
        GameObject g = stage.HasValue ? m_StartMenuObj : m_PersistentUIObj;
        if (stage.HasValue)
        {
            switch (stage)
            {
                case GameStage.MatchConfig:
                    g = m_MatchConfigMenuObj;
                    break;
                case GameStage.PlayerConfig:
                    g = m_PlayerConfigMenuObj;
                    break;
                default:
                    break;
            }
        }

        TogglablePanels
            .Select(go =>
            {
                Debug.Log($"found togglable {go}");
                return go;
            })
            .Where(go => go != g)
            .Select(go => {
                Debug.Log($"...shutting down {go}");
                go.SetActive(false);
                return 0;
            });
        g.SetActive(true);
        PlayPanelSfx(g);
    }

    private void PlayPanelSfx(GameObject src)
    {
        RuntimeManager.PlayOneShot(panelSound, src.transform.position);
    }

    private void Awake()
    {
        Instance = this;
    }
}
