using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkDisabler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Disables non-owner handActual scripts
        if (IsClient && !IsOwner) {
            var handActualScripts = GetComponentsInChildren<HandActual>();
            foreach (HandActual ha in handActualScripts) {
                ha.enabled = false;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
