using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject trackingObject;
    public GameObject ball;
    private Vector3 firstVector;
    private Vector3 sumVector;
    private bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        


    }
    

    // Update is called once per frame
    void Update()
    {
        while (true)
        {
            if (isFirst)
            {
                firstVector = trackingObject.transform.position;
                isFirst = false;
            }
            sumVector += trackingObject.transform.position;

            ball.transform.position = trackingObject.transform.position;


        }

        Debug.Log("First Vector = " + firstVector);
        Debug.Log("Sum Vector = " + sumVector);





    }
}
