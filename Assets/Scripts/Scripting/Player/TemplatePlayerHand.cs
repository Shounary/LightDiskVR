using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class TemplatePlayerHand : MonoBehaviour
{
    // one of the controllers
    private InputDevice targetDevice;

    // a reference to a spawned hand
    private GameObject spawnedModel;

    // hand prefab, either right or left
    public GameObject handPrefab;

    public InputDeviceCharacteristics controllerCharacteristics;



    private Animator handAnimator;

    // at the start we look through the devices that are mapped to the "inputDevices" variable. Once this is done we can actually read the input from the controller.
    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        Debug.Log("devices: " + inputDevices.Count);
        if (inputDevices.Count > 0) {
            targetDevice = inputDevices[0];
            spawnedModel = Instantiate(handPrefab, transform);
        } else {
            Debug.LogWarning("Controller not found!");
            targetDevice = inputDevices[0];
            Instantiate(handPrefab, transform);
        }

        handAnimator = spawnedModel.GetComponent<Animator>();
    }

    void Update() {
        UpdateAnimation();
    }

    // Checks if the player presses the grip or a trigger and updates the animation
    private void UpdateAnimation() {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger)) {
            handAnimator.SetFloat("Trigger", trigger);
        } else {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float grip)) {
            grip = Mathf.Clamp(grip, 0f, 0.25f);
            handAnimator.SetFloat("Grip", grip);
        } else {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
