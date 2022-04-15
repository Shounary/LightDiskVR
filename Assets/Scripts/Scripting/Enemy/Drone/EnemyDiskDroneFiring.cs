using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDiskDroneFiring : MonoBehaviour
{
    [Header("Internal References")]
    public GameObject deathEffect;
    public GameObject enemyDiskPrefab;
    public Transform firePoint;
    public Transform playerTarget;

    [Header("Attack Parameters")]
    public float attackRate = 0.2f;
    public float initialDiskSpeed = 12f;
    public float attackRange = 50f;
    public float diskLifetime = 2.0f;
    public float aimDeviationX = 1.0f;
    public float aimDeviationY = 1.0f;
    public float aimDeviationZ = 1.0f;

    [Header("Other")]
    public LayerMask doNotFireThrough;
    public LayerMask toTakeDamageFrom;

    private float attackCooldown;
    private bool hasDisk;
    private GameObject enemyDisk;
    //private Vector3[] reflectionTargets;

    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = 1 / attackRate;
        hasDisk = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTarget);
        attackCooldown -= Time.deltaTime;
        if (attackCooldown < 0 && HasLineOfSight(playerTarget) && HasDisk()) {
            FireDiskStraight(playerTarget);
            attackCooldown = 1 / attackRate;
        }
    }

    public bool HasLineOfSight(Transform target) {
        return !Physics.Raycast(firePoint.position, target.position - firePoint.position, attackRange, doNotFireThrough);
    }

    public bool HasDisk() {
        return true;
    }

    public void FireDiskStraight(Transform target) {
        enemyDisk = Instantiate(enemyDiskPrefab, firePoint.position, firePoint.rotation);
        float offsetX = Random.Range(-1 * aimDeviationX, aimDeviationX);
        float offsetY = Random.Range(-1 * aimDeviationY, aimDeviationY);
        float offsetZ = Random.Range(-1 * aimDeviationZ, aimDeviationZ);
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        enemyDisk.GetComponent<Rigidbody>().velocity = initialDiskSpeed * (target.position - firePoint.position + offset).normalized;
        Destroy(enemyDisk, diskLifetime);
    }

    private void OnTriggerEnter(Collider other) {
        if (toTakeDamageFrom == (toTakeDamageFrom | (1 << other.gameObject.layer))) {
            Destroy(Instantiate(deathEffect, transform.position, transform.rotation), 3f);
            Destroy(gameObject);
        }
    }
}
