using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    private GameObject drawpoint; 
    private bool isDrawing;
    private GameObject lineRendererParent;
    private LineRenderer lineRenderer;
    private bool conditionToDraw;

    // Start is called before the first frame update
    void Start()
    {
        conditionToDraw = true;//Set condition if a paticular button is clicke
        lineRenderer = GetComponent<LineRenderer>();
        drawpoint = GameObject.FindGameObjectWithTag("ld");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Draw()
    {
        if (conditionToDraw)
        {
            isDrawing = true;
            lineRendererParent = new GameObject();
            lineRendererParent.tag = "protectection";
            AddForce(lineRendererParent);
            lineRendererParent.transform.position = transform.position;
            lineRenderer = lineRendererParent.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, drawpoint.transform.position);
            lineRenderer.SetPosition(0, drawpoint.transform.position);
        }
        if (isDrawing)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount++, drawpoint.transform.position);
            }
        }

    }
    private void AddForce(GameObject l)
    {
        Debug.Log("Force to be added");
    }
}
