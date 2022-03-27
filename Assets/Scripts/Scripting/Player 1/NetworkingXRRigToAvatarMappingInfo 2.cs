using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingXRRigToAvatarMappingInfo : MonoBehaviour
{
    public Transform mainCamera;
    public Transform leftHand;
    public Transform rightHand;

    public static NetworkingXRRigToAvatarMappingInfo instance;

    private bool rigAlreadyMapped;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigAlreadyMapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsMapped() {
        return rigAlreadyMapped;
    }

    public void SetMapped(bool isMapped) {
        rigAlreadyMapped = isMapped;
    }
}
