using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour
{
    public Rigidbody rb;
    public float t;
    public int pointReward;
    public GameObject destructionEffect;

    private void OnCollisionEnter(Collision collision) {
        if (ShootingRangeModeManager.instance != null)
            ShootingRangeModeManager.instance.increaseScoreIfGameNotOver(pointReward);
        Destroy(Instantiate(destructionEffect, gameObject.transform.position, gameObject.transform.rotation), 2.2f);
        Destroy(gameObject);
    }

    private void Start() {
        t = 0.0f;
    }

    // makes the target ocsilate
    private void Update() {
        rb.velocity = new Vector3(0f, 0.25f * Mathf.Sin(2 * t), 0f);
        t += Time.deltaTime;
    }
}
