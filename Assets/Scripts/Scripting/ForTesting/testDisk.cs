using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDisk : MonoBehaviour
{
    public Rigidbody diskRB;
    public Vector3 initSpeed;
    // Start is called before the first frame update
    void Start()
    {
        diskRB.velocity = initSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
