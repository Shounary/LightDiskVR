using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
<<<<<<< Updated upstream
using FMODUnity;
=======
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
    private void Start()
    {
        UpdatePanelDisplay(null);
    }

=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        UpdatePanelDisplay(null);

=======
>>>>>>> Stashed changes
        var nmHolder = NetworkManager.Singleton.gameObject;
        NetworkManager.Singleton.Shutdown();
        Destroy(nmHolder);
    }

<<<<<<< Updated upstream
    public void UpdatePanelDisplay(GameStage? stage)
    {
        GameObject g = stage.HasValue ? m_PersistentUIObj : m_StartMenuObj;
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

        foreach (GameObject go in new GameObject[]{
                m_PlayerConfigMenuObj,
                m_MatchConfigMenuObj,
                m_StartMenuObj
            })
        {
            go.SetActive(go == g);
        }
        g.SetActive(true);
        PlayPanelSfx(g);
    }

    private void PlayPanelSfx(GameObject src)
    {
        RuntimeManager.PlayOneShot("event:/UI/Simple_Hover", src.transform.position);
    }

=======
>>>>>>> Stashed changes
    private void Awake()
    {
        Instance = this;
    }
}
