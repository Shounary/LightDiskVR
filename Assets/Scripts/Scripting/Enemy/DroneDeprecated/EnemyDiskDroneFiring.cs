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
        enemyDisk.GetComponent<Rigidbody>().velocity = initialDiskSpeed * Vector3.Normalize(target.position - firePoint.position - new Vector3(0, -Random.Range(-0.5f * target.position.y, -0.15f * target.position.y), 0));
        Destroy(enemyDisk, 2f);
    }

    private void OnTriggerEnter(Collider other) {
        if (toTakeDamageFrom == (toTakeDamageFrom | (1 << other.gameObject.layer))) {
            Destroy(Instantiate(deathEffect, transform.position, transform.rotation), 3f);
            Destroy(gameObject);
        }
    }
}
