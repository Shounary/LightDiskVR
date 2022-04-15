using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSpawner : MonoBehaviour
{
    public float initSpawnRate = 0.2f;
    public float maxSpawnRate = 10.0f;
    public float spawnRateIncrease = 0.2f;

    public List<Transform> droneZones;
    public GameObject enemyDronePrefab;
    public Transform droneTarget;

    private float counter;
    private float currentSpawnRate;

    void Start() {
        currentSpawnRate = initSpawnRate;
        counter = 1f / currentSpawnRate;
    }

    void Update()
    {
        if (currentSpawnRate < maxSpawnRate)
            currentSpawnRate += spawnRateIncrease * Time.deltaTime;

        if (counter <= 0f) {
            SpawnDrone();
            counter = 1f / currentSpawnRate;
        } else {
            counter -= Time.deltaTime;
        }
    }

    public void SpawnDrone() {
        int i = Random.Range(0, droneZones.Count);
            Vector3 spawnpoint = droneZones[i].position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
            GameObject enemy = Instantiate(enemyDronePrefab, spawnpoint, Quaternion.identity);

            EnemyDiskDroneFiring droneWeapon = enemy.GetComponent<EnemyDiskDroneFiring>();
            droneWeapon.playerTarget = droneTarget;
            
            EnemyMovement droneMovement = enemy.GetComponent<EnemyMovement>();
            droneMovement.moveArea = droneZones[i];

            NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
            navMeshAgent.baseOffset = Random.Range(0.8f, 3.4f);
    }
}
