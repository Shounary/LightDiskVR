using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PreMatchUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StartMenuObj, m_MatchConfigMenuObj, m_PlayerConfigMenuObj, m_PersistentUIObj;
    public static PreMatchUIManager Instance { get; private set; }

    public static GameObject StartMenuObj => Instance.m_StartMenuObj;
    public static StartMenuUIFlat StartMenu => StartMenuObj.GetComponent<StartMenuUIFlat>();

    public static GameObject MatchConfigMenuObj => Instance.m_MatchConfigMenuObj;
    public static MatchConfigMenuUIFlat MatchConfigMenu => MatchConfigMenuObj.GetComponent<MatchConfigMenuUIFlat>();

    public static GameObject PlayerConfigMenuObj => Instance.m_PlayerConfigMenuObj;
    public static PlayerConfigMenuUIFlat PlayerConfigMenu => PlayerConfigMenuObj.GetComponent<PlayerConfigMenuUIFlat>();

    public static GameObject PersistentUIObj => Instance.m_PersistentUIObj;
    public static PreMatchPersistentUI Persistent => PersistentUIObj.GetComponent<PreMatchPersistentUI>();

    public static void ResumeStart()
    {
        StartMenuObj.SetActive(true);
        MatchConfigMenuObj.SetActive(false);
        PlayerConfigMenuObj.SetActive(false);
        PersistentUIObj.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }
}
