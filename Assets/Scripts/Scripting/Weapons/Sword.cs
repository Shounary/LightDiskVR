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
    public float slashSpeed = 20f;
    public Transform startPoint;
    public GameObject blade;

    private LineRenderer lineRend;
    private GameObject lineRendererGOPointer;
    private bool energyMode;

    private List<Vector3> pointVectors;
    private List<Vector3> dirVectors;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
        energyMode = false;
    }

    // uses trigger btn to draw
    public override void TriggerFunction(float additionalFactor, Transform targetTransform)
    {
        energyMode = true;
    }

    public void Draw()
    {
        if ((pointVectors.Count <= 10) && ((pointVectors.Count == 0) || 
            ((Vector3.Distance(pointVectors[pointVectors.Count - 1], startPoint.position) > 0.2))))
        {
            if ((pointVectors.Count == 0) || (Vector3.Dot((startPoint.position - pointVectors[pointVectors.Count - 1]), startPoint.up) < 0))
            {
                pointVectors.Add(startPoint.position);
                dirVectors.Add(startPoint.forward);
            }
            else
            {
                if (energyMode)
                {
                    CreateProjectileFromDrawing();
                }
                else
                {
                    Destroy(lineRendererGOPointer);
                }
                Reset();
            }
        }
        else if (pointVectors.Count > 3)
        {
            if (energyMode)
            {
                CreateProjectileFromDrawing();
            }
            else
            {
                Destroy(lineRendererGOPointer);
            }
            Reset();
        }
        else if (pointVectors.Count >= 1)
        {
            pointVectors.RemoveAt(0);
            dirVectors.RemoveAt(0);

        } 
        if (pointVectors.Count > 1)
        {
            lineRend.positionCount = pointVectors.Count;
            lineRend.SetPositions(pointVectors.ToArray());
        }
    }

    // resets the prev drawing
    public void Reset()
    {
        lineRendererGOPointer = (GameObject)Instantiate(swordLineRendererGO, Vector3.zero, Quaternion.identity);
        lineRend = lineRendererGOPointer.GetComponent<LineRenderer>();
        pointVectors = new List<Vector3>();
        dirVectors = new List<Vector3>();
    }

    public void CreateProjectileFromDrawing()
    {
        SwordSlash swordSlash = lineRendererGOPointer.GetComponent<SwordSlash>();
        swordSlash.parentBlade = blade;
        Vector3[] fieldPoints = new Vector3[lineRend.positionCount];
        Vector3 totalDir = new Vector3(0, 0, 0);
        lineRend.GetPositions(fieldPoints);
        foreach (Vector3 fieldPoint in fieldPoints)
        {
            SphereCollider sphereCollider = lineRendererGOPointer.AddComponent<SphereCollider>();
            sphereCollider.center = fieldPoint;
            sphereCollider.radius = sphereColliderRadius;
            //sphereCollider.isTrigger = true;
        }
        for (int i = 0; i < dirVectors.Count; i++)
        {

            totalDir += (dirVectors[i] * (i + 1));
        }
        Rigidbody body = lineRendererGOPointer.GetComponent<Rigidbody>();
        body.AddForce(totalDir.normalized * slashSpeed);
    }
}
