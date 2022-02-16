using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class NetworkingLeftHand : MonoBehaviour
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


    private Rigidbody GetTargetDisk(Transform controllerTransform)
    {
        return ArenaInfo.instance.playerDisks[0]; // BETTER version to be implemented
    }

    private void UpdateAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger))
        {
            NetworkManager.Singleton.StartHost();     
        }
    }
}
