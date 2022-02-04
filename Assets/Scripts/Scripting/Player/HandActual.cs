using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class HandActual : MonoBehaviour
{
    private InputDevice targetDevice;
    private GameObject spawnedModel;

    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;

    public float diskReturnForceMagnitude = 5f;
    public float stoppingFactorMultiplier = 0.2f;

    private Animator animator;
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

        animator = spawnedModel.GetComponent<Animator>();
    }

    void Update() {
        UpdateAnimation();
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger) && trigger > 0.5) {
            AttractDisk(trigger);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool pressed) && pressed) {
            LevelManager.instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void AttractDisk(float additionalFactor) {
        Rigidbody targetDisk = GetTargetDisk(transform);
        Vector3 targetDirection = Vector3.Normalize(transform.position - targetDisk.position);
        Vector3 initialDirection = Vector3.Normalize(targetDisk.velocity);
        float angle = Vector3.Angle(targetDirection, initialDirection);

        Vector3 normal = additionalFactor * stoppingFactorMultiplier * diskReturnForceMagnitude * Time.deltaTime * (-1) * Vector3.Magnitude(targetDisk.velocity) * Mathf.Abs(Mathf.Sin(Mathf.Abs(angle))) * initialDirection;
        Vector3 parallel = additionalFactor * diskReturnForceMagnitude * Time.deltaTime * targetDirection;

        if (angle > 5) {
            targetDisk.AddForce(normal, ForceMode.VelocityChange);
        }

        targetDisk.AddForce(parallel, ForceMode.VelocityChange);
    }

    private Rigidbody GetTargetDisk(Transform controllerTransform) {
        return ArenaInfo.instance.playerDisks[0]; // BETTER version to be implemented
    }

    private void UpdateAnimation() {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger)) {
            animator.SetFloat("Trigger", trigger);
        } else {
            animator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float grip)) {
            grip = Mathf.Clamp(grip, 0f, 0.25f);
            animator.SetFloat("Grip", grip);
        } else {
            animator.SetFloat("Grip", 0);
        }
    }
}
