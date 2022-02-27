using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StasisDisk : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;

    private InputDevice targetDevice;

    private Vector3 storedVelocity;
    private bool inStasis;
    private Rigidbody body;
    

    // Start is called before the first frame update
    void Start()
    {
        // Set up hands
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);
        if (inputDevices.Count > 0) {
            targetDevice = inputDevices[0];
        }

        // Set up Stasis physics stuff
        inStasis = false;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool trigger)) {
            startStasis();
        } else {
            endStasis();
        }
    }

    public void startStasis() {
        if (!inStasis) {
            inStasis = true;
            storedVelocity = body.velocity;
            body.velocity = body.velocity / 5;
        }
    }

    public void endStasis() {
        if (inStasis) {
            inStasis = false;
            body.velocity = storedVelocity;
        }
    }
}
