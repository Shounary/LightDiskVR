using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public enum Hand{
    LEFT,
    RIGHT,
    NONE
};

public class HandActual : MonoBehaviour
{
    private InputDevice targetDevice;
    private GameObject spawnedModel;

    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;
    public Hand hand;
    public WeaponInventory weaponInventory;
    public Weapon weapon;

    public bool button1Pressed;
    public bool button2Pressed;
    public bool stickDelay;

    private Animator animator;
    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        Debug.Log("devices: " + inputDevices.Count);
        if (inputDevices.Count > 0) {
            targetDevice = inputDevices[0];
            //spawnedModel = Instantiate(handPrefab, transform);
        } else {
            Debug.LogWarning("Controller not found!");
            //targetDevice = inputDevices[0];
            //Instantiate(handPrefab, transform);
        }
        //weaponInventory = GetComponentInParent<WeaponInventory>();
        //animator = spawnedModel.GetComponent<Animator>();
    }

    void Update() {
        if(weaponInventory == null)
        {
            weaponInventory = GetComponentInParent<WeaponInventory>();
        }
        weapon = weaponInventory.getActiveWeapon(hand);
        //UpdateAnimation();
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger) && trigger > 0.5) {
            if(weapon != null)
            {
                weapon.TriggerFunction(trigger, this.transform);
            }
        }

        // Grip/Summon
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float grip) && grip > 0.5) {
            if(weapon != null)
            {
                weapon.GrabHeldFunction(grip, this.transform);
            }
        }
        
        if( targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed1) && pressed1 && !button1Pressed) {
            if(PauseController.instance.isDead)
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            button1Pressed = true;
            Debug.Log("Hello");
            //weapon.MainButtonFunction();
            if(weapon != null && weapon.isHeld) {
            weaponInventory.ToggleSelectUI(hand);
            }
        }
        else if (!pressed1 && button1Pressed)
        {
            weapon.MainButtonReleaseFunction();
            button1Pressed = false;
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed2) && pressed2) {
            if(weapon != null) {
                weapon.MainButtonFunction();
               // weapon.SecondaryButtonFunction();
            }
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool pressed) && pressed) {
            LevelManager.instance.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystick) && Mathf.Abs(joystick.x) > 0.5 && !stickDelay) {
            stickDelay = true;
            weaponInventory.cycleWeaponList(hand, joystick.x > 0 ? 1: -1);
            StartCoroutine(JoystickTimerCoroutine());
        }
    }

    IEnumerator JoystickTimerCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
        stickDelay = false;
    }

/*
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
        return weaponInventory.getActiveWeapon(hand).GetComponent<Rigidbody>();
    }
*/
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
