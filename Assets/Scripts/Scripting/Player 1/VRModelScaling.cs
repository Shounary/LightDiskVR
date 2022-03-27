using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModelScaling : MonoBehaviour
{
    // these values are assumed to be in cm, but shouldn't actually matter if they are consistent
    public float playerHeight = 165;
    public float modelHeight = 183; // TODO: figure out the model's actual default height

    // Start is called before the first frame update
    void Start()
    {
        float ratio = playerHeight / modelHeight;
        transform.localScale = transform.localScale * ratio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
