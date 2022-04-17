using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectingBullet : MonoBehaviour
{
    [Header("Parameters")]
    public float[] deflectXRange = { 0.8f, 1.2f };
    public float[] deflectYRange = { 0.8f, 1.2f };
    public float[] deflectZRange = { 0.8f, 1.2f };
    public LayerMask bulletDeflectsFrom;

    [Header("References")]
    public GameObject destroyEffect;
    public Material newBulletMaterial;
    public Rigidbody bulletRb;

    private bool isDeflected;

    private void Start()
    {
        isDeflected = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletDeflectsFrom == (bulletDeflectsFrom | 1 << collision.collider.gameObject.layer)) {
            if (!isDeflected)
            {
                SetDeflectedBulletVelocity();
                SetDeflectedBulletMaterial();
                isDeflected = true;
            }
        } else
        {
            Destroy(Instantiate(destroyEffect, transform.position, Quaternion.identity), 0.6f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletDeflectsFrom == (bulletDeflectsFrom | 1 << other.gameObject.layer))
        {
            if (!isDeflected)
            {
                SetDeflectedBulletVelocity();
                SetDeflectedBulletMaterial();
                isDeflected = true;
            }
        } else
        {
            Destroy(Instantiate(destroyEffect, transform.position, Quaternion.identity), 0.6f);
            Destroy(gameObject);
        }
    }

    public void SetDeflectedBulletVelocity()
    {
        float x = Random.Range(deflectXRange[0], deflectXRange[1]) * bulletRb.velocity.x;
        float y = Random.Range(deflectYRange[0], deflectYRange[1]) * bulletRb.velocity.y;
        float z = Random.Range(deflectZRange[0], deflectZRange[1]) * bulletRb.velocity.z;
        bulletRb.velocity = -1 * new Vector3(x, y, z);
    }

    public void SetDeflectedBulletMaterial() {
        GetComponent<Renderer>().material = newBulletMaterial;
    }
}
