using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    private InputDevice rightController;
    private InputDevice leftController;

    //private GameObject handGO;

    public GameObject rightHandPrefab; 
    public float forceMagnitude = 5f;
    public Rigidbody[] playerDisks;
    public Transform rightHandControllerTransform;
    public Transform leftHandControllerTransform;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> rightInputDevices = new List<InputDevice>();
        List<InputDevice> leftInputDevices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right;
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, rightInputDevices);
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftInputDevices);

        if (rightInputDevices.Count > 0) {

            rightController = rightInputDevices[0];
            //handGO = Instantiate(handGO);
        }
        if (leftInputDevices.Count > 0)
            leftController = leftInputDevices[0];
    }

    // Update is called once per frame
    void Update() {
        if (rightController.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerLevel) && rightTriggerLevel > 0.5) {
            RightHandAttractDisk(rightTriggerLevel);
        } else if (leftController.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerLevel) && leftTriggerLevel > 0.5) {
            LeftHandAttractDisk(leftTriggerLevel);
        }
    }

    private void RightHandAttractDisk(float additionalFactor) {
        Rigidbody targetDisk = GetTargetDisk(rightHandControllerTransform);
        Vector3 attractionDirection = additionalFactor * forceMagnitude * Time.deltaTime * Vector3.Normalize(rightHandControllerTransform.position - targetDisk.position);
        targetDisk.AddForce(attractionDirection, ForceMode.VelocityChange);
    }

    private void LeftHandAttractDisk(float additionalFactor) {
        Rigidbody targetDisk = GetTargetDisk(leftHandControllerTransform);
        Vector3 attractionDirection = additionalFactor * forceMagnitude * Time.deltaTime * Vector3.Normalize(leftHandControllerTransform.position - targetDisk.position);
        targetDisk.AddForce(attractionDirection, ForceMode.VelocityChange);
    }

    private Rigidbody GetTargetDisk(Transform controllerTransform) {
        return playerDisks[0]; // BETTER version to be implemented
    }
}
