using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class RaycastSceneChangeUpdate : MonoBehaviour
{
    // Fixes the issue of of raycast from controller getting stuck when the scene changes
    private XRRayInteractor controller;
    void Start()
    {
        controller = GetComponent<XRRayInteractor>();
        SceneManager.activeSceneChanged += OnSceneChangedUpdateRaycast;
    }

    void Update()
    {
        if (!controller.enabled) {
            controller.enabled = true;
        }
    }

    private void OnSceneChangedUpdateRaycast(Scene curr, Scene next) {
        controller.enabled = false;
    }
}
