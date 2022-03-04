using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class HandPauseController : MonoBehaviour
{
    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;
    private bool mBPressed_buffer = false;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);
        if (inputDevices.Count != 0)
            targetDevice = inputDevices[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice == null)
            return;
        if (targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed_) && pressed_ && !mBPressed_buffer)
        {
            mBPressed_buffer = true;
            try
            {
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<BaseAccessor>().ActivatePause();
            } catch { }
        } else
        {
            mBPressed_buffer = false;
        }
    }
}
