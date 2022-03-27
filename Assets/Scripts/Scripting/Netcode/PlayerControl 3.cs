using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();

    //client caching
    private float oldForwardBackPosition;
    private float oldLeftRightPosition;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0,
            Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + leftRightPosition.Value, transform.position.y, 
            transform.position.z + leftRightPosition.Value);
    }

    private void UpdateClient()
    {
        float forwardBackward = 0;
        float leftRight = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            forwardBackward += walkSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            forwardBackward -= walkSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRight -= walkSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRight += walkSpeed;
        }
    }
}
