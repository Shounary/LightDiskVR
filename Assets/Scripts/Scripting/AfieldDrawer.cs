using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class AfieldDrawer : Weapon
{
    public Transform controllerTransform;
    public GameObject aFieldLineRendererGO;
    public float timeBeforeButtonReleaseRegisters = 0.3f;

    private LineRenderer lineRend;
    private Transform startPoint;
    private GameObject lineRendererGOPointer;

    private bool isDrawing;
    private bool drawingCompleted;

    private float timerCounter;

    // Start is called before the first frame update
    void Start()
    {
        isDrawing = false;
        drawingCompleted = true;
    }

    // Update is called once per frame
    void Update()
    { 
        if (drawingCompleted) {
            return;
        }

        // checks whether the drawing is actually complete by waiting for "timeBeforeButtonReleaseRegisters" seconds
        if (isDrawing) {
            isDrawing = false;
            timerCounter = timeBeforeButtonReleaseRegisters;
        } else {
            if (timerCounter <= 0) {
                drawingCompleted = true;
                // TODO: some function to create a field from drawing, also destroy the reference to a previous one.
            } else {
                timerCounter -= Time.deltaTime;
            }
        }
    }

    // uses trigger btn to draw
    public override void TriggerFunction(float additionalFactor, Transform targetTransform)
    {
        if (drawingCompleted) {
            Reset();
            startPoint = controllerTransform;
            lineRendererGOPointer = (GameObject) Instantiate(aFieldLineRendererGO);
            lineRend = lineRendererGOPointer.GetComponent<LineRenderer>();
            lineRend.SetPosition(0, controllerTransform.position);
            lineRend.SetPosition(1, controllerTransform.position);
        }
        isDrawing = true;
        drawingCompleted = false;
        Draw();
    }

    public void Draw() {
        lineRend.SetPosition(lineRend.positionCount++, controllerTransform.position);
    }

    // resets the prev drawing
    public void Reset() {
        if (lineRendererGOPointer == null)
            return;
        lineRend = null;
        startPoint = null;
        Destroy(lineRendererGOPointer);
        lineRendererGOPointer = null;
    }
}
