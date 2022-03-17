using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw : MonoBehaviour
{
    public Transform transformHand;
    public LineRenderer lineRenderer;
    private Vector3[] positionArray = new Vector3[2];
    private bool isFirst;


    public void create()
    {
        if (isFirst)
        {
            positionArray[1] = transformHand.position;
            isFirst = false;

        }
        else
        {
            positionArray[0] = positionArray[1];
            positionArray[1] = transformHand.position;
            lineRenderer.SetPositions(positionArray);
        }
        Debug.Log("Pos1 = " + positionArray[0] + "\n" + "Pos2 = " + positionArray[1]);

    }

    void Start()
    {

    }

  
    void Update()
    {  
       create(); 
    }
}
