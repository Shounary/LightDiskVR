using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;


public class SpawnEnemyScript : MonoBehaviour
{
        private InputDevice targetDevice;
        public InputDeviceCharacteristics controllerCharacteristics;
        

    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);
        targetDevice = inputDevices[0];

        //weaponInventory = GetComponentInParent<WeaponInventory>();
        //animator = spawnedModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed2) && pressed2) {
            SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        }

    }
}
