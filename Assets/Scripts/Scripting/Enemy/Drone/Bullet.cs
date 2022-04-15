using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject destroyEffect;

    void OnTriggerEnter(Collider other) {
        Destroy(Instantiate(destroyEffect, transform.position, Quaternion.identity), 0.6f);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        Destroy(Instantiate(destroyEffect, transform.position, Quaternion.identity), 0.6f);
        Destroy(gameObject);
    }
}
