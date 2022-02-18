using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class NetworkingRightHand : MonoBehaviour
{
    private InputDevice targetDevice;
    private GameObject spawnedModel;

    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;


    private Animator animator;
    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        Debug.Log("devices: " + inputDevices.Count);
        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
            spawnedModel = Instantiate(handPrefab, transform);
        }
        else
        {
            Debug.LogWarning("Controller not found!");
            targetDevice = inputDevices[0];
            Instantiate(handPrefab, transform);
        }

        animator = spawnedModel.GetComponent<Animator>();
    }

    private void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger))
        {
            NetworkManager.Singleton.StartClient();     
        }
    }
}
