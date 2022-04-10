using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour
{
    [Header("Parameters")]
    public int pointReward;
    public float maxSpeed = 1f;
    public float timeOffset = 0f;
    public float period = 1f;

    public Rigidbody rb;
    public GameObject destructionEffect;

    private float t;

    private void OnCollisionEnter(Collision collision) {
        if (ShootingRangeModeManager.instance != null)
            ShootingRangeModeManager.instance.increaseScoreIfGameNotOver(pointReward);
        Destroy(Instantiate(destructionEffect, gameObject.transform.position, gameObject.transform.rotation), 2.2f);
        Destroy(gameObject);
    }

    private void Start() {
        t = 0.0f + timeOffset;
    }

    // makes the target ocsilate
    private void Update() {
        rb.velocity = new Vector3(0f, maxSpeed * Mathf.Sin(2 * t / period), 0f);
        t += Time.deltaTime;
    }
}
