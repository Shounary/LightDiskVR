using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAvatarRigMapper : MonoBehaviour
{
    public VRRig vrRig;

    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkingXRRigToAvatarMappingInfo.instance.IsMapped()) {
            vrRig.head.vrTarget = NetworkingXRRigToAvatarMappingInfo.instance.mainCamera;
            vrRig.leftHand.vrTarget = NetworkingXRRigToAvatarMappingInfo.instance.leftHand;
            vrRig.rightHand.vrTarget = NetworkingXRRigToAvatarMappingInfo.instance.rightHand;
            NetworkingXRRigToAvatarMappingInfo.instance.SetMapped(true);
        }
    }
}
