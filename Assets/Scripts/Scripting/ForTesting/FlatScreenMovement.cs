using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatScreenMovement : MonoBehaviour
{
    private Vector3 newPos;
    void Update()
    {
        newPos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            newPos += new Vector3(0, 0, 1 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPos += new Vector3(1 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPos -= new Vector3(0, 0, 1 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPos -= new Vector3(1 * Time.deltaTime, 0, 0);
        }

        transform.position = newPos;
    }
}
