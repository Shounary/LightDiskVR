using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ManagerSaver : MonoBehaviour
{
    public GameObject xrInteractionManager;
    public GameObject inputManager;
    public GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(xrInteractionManager);
        DontDestroyOnLoad(inputManager);
        DontDestroyOnLoad(eventSystem);
    }
}
