using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class DirectInteractorSceneChangeUpdate : MonoBehaviour
{
    // Updates the interactor to solve the issue arising from transitioning from one scene to the next
    private XRDirectInteractor interactor;
    void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
        SceneManager.activeSceneChanged += OnScheneChangedUpdateInteractor;
    }

    void Update()
    {
        if (!interactor.enabled) {
            interactor.enabled = true;
        }
    }

    private void OnScheneChangedUpdateInteractor(Scene curr, Scene next) {
        interactor.enabled = false;
    }
}
