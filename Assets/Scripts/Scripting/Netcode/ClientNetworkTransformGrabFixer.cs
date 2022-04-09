using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Samples;

public class ClientNetworkTransformGrabFixer : MonoBehaviour
{
    public ClientNetworkTransform weaponCNT;
    private bool setNext = false;

    void Update()
    {
        // if (setNext) {
        //     weaponCNT.Interpolate = true;
        //     setNext = false;
        // }

        // if (!weaponCNT.Interpolate) {
        //     setNext = true;
        // }
        
        //weaponCNT.SyncPositionX = true;
        //weaponCNT.SyncPositionY = true;
        //weaponCNT.SyncPositionZ = true;
    }
}
