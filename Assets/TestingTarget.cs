using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTarget : MonoBehaviour
{
    public Transform deathEffect;

    private void OnCollisionEnter(Collision collision) {
        Destroy(Instantiate(deathEffect, gameObject.transform.position, gameObject.transform.rotation), 2f);
        Destroy(gameObject);
    }
}
