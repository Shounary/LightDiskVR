using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();

    //client chachhing
    private float oldForwardBackPosition;
    private float oldLeftRightPosition;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0,
            Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateServer();
        }

        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }

    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + leftRightPosition.Value,
            transform.position.y, transform.position.z + forwardBackPosition.Value);
    }

    private void UpdateClient()
    {
        float forwardBackward = 0;
        float leftRight = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            forwardBackward += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRight += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            forwardBackward -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRight -= speed * Time.deltaTime;
        }

        if (oldForwardBackPosition != forwardBackward ||
            oldLeftRightPosition != leftRight)
        {
            oldForwardBackPosition = forwardBackward;
            oldLeftRightPosition = leftRight;

            //Update the server
            UpdateClientPositionServerRpc(forwardBackward, leftRight);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight)
    {
        forwardBackPosition.Value = forwardBackward;
        leftRightPosition.Value = leftRight;
    }
}
