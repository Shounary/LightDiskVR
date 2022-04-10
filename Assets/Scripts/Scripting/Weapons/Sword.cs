using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class Sword : Weapon
{
    public GameObject swordLineRendererGO;
    public float timeBeforeButtonReleaseRegisters = 0.3f;
    public float sphereColliderRadius = 0.1f;
    public Transform startPoint;

    private LineRenderer lineRend;
    private GameObject lineRendererGOPointer;

    private bool isDrawing;
    private bool drawingCompleted;

    private float timerCounter;
    private List<Vector3> fieldPointVectors;

    // Start is called before the first frame update
    void Start()
    {
        fieldPointVectors = new List<Vector3>();
        isDrawing = false;
        drawingCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawingCompleted)
        {
            return;
        }

        // checks whether the drawing is actually complete by waiting for "timeBeforeButtonReleaseRegisters" seconds
        if (isDrawing)
        {
            isDrawing = false;
            timerCounter = timeBeforeButtonReleaseRegisters;
        }
        else
        {
            if (timerCounter <= 0)
            {
                drawingCompleted = true;
                // CreateProjectileFromDrawing();
                // TODO: some function to create a field from drawing, also destroy the reference to a previous one.
            }
            else
            {
                timerCounter -= Time.deltaTime;
            }
        }
    }

    // uses trigger btn to draw
    public override void TriggerFunction(float additionalFactor, Transform targetTransform)
    {
        if (!isHeld)
        {
            return;
        }

        if (drawingCompleted)
        {
            Reset();
            lineRendererGOPointer = (GameObject)Instantiate(swordLineRendererGO, Vector3.zero, Quaternion.identity);
            lineRend = lineRendererGOPointer.GetComponent<LineRenderer>();
            lineRend.SetPosition(0, startPoint.position);
            lineRend.SetPosition(1, startPoint.position);
        }
        isDrawing = true;
        drawingCompleted = false;
        Draw();
    }

    public void Draw()
    {
        if ((fieldPointVectors.Count == 0) || (Vector3.Distance(fieldPointVectors[fieldPointVectors.Count - 1], startPoint.position) > 0.05))
        {
            if (fieldPointVectors.Count >= 30)
            {
                fieldPointVectors.RemoveAt(0);
            }
            fieldPointVectors.Add(startPoint.position);
            lineRend.positionCount = fieldPointVectors.Count;
            lineRend.SetPositions(fieldPointVectors.ToArray());
        }
    }

    // resets the prev drawing
    public void Reset()
    {
        if (lineRendererGOPointer == null)
            return;
        lineRend = null;
        Destroy(lineRendererGOPointer);
        lineRendererGOPointer = null;
        fieldPointVectors = new List<Vector3>();
    }

    public void CreateProjectileFromDrawing()
    {
        AccelerationField accelerationField = lineRendererGOPointer.GetComponent<AccelerationField>();
        accelerationField.SetForceVector(CalculateForceVector());

        Vector3[] fieldPoints = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(fieldPoints);
        foreach (Vector3 fieldPoint in fieldPoints)
        {
            SphereCollider sphereCollider = lineRendererGOPointer.AddComponent<SphereCollider>();
            sphereCollider.center = fieldPoint;
            sphereCollider.radius = sphereColliderRadius;
            sphereCollider.isTrigger = true;
            // TODO add an internal collider at some point
        }
    }

    // calculates the direction in which the A-field applies the force based on the average of fieldPointVectors array. 
    public Vector3 CalculateForceVector()
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 v3 in fieldPointVectors)
        {
            sum += v3;
        }
        return sum / fieldPointVectors.Count;
    }
}
