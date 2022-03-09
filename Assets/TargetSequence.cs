using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSequence : MonoBehaviour
{
    [Header("Blue Target")]
    public GameObject blueTargetModel;
    public GameObject spawnBlueTargetEffect;
    public GameObject despawnBlueTargetEffect;
    public float despawnBlueEffectTime;

    [Header("Orange Target")]
    public GameObject orangeTargetModel;
    public GameObject spawnOrangeTargetEffect;
    public GameObject despawnOrangeTargetEffect;
    public float despawnOrangeEffectTime;

    
    public Transform[] blueSpawnPoints; //blue swawn points across different rounds
    public Transform[] orangeSpawnPoints; //blue swawn points across different rounds

    public float[] timeBetweenWaves;
    public float[] objectSpawnTime;

    private int currentWave;
    private float waveTimer;
    private float spawnTimer;
    private List<GameObject> activeBlueObjects;
    private List<GameObject> activeOrangeObjects;

    void Start()
    {
        activeBlueObjects = new List<GameObject>();
        activeOrangeObjects = new List<GameObject>();
        currentWave = 0;
        waveTimer = 1f;
        spawnTimer = 2f;
    }

    void Update()
    {
        if (currentWave >= timeBetweenWaves.Length - 1) {
            Debug.Log("LOLLLLLLLLLLLLLLLLLLLLLLLLL");
            return;
        }
        Debug.Log("WTFFFFFFFFFFFFFFFFFFFFF");
        waveTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        Debug.Log("wave timer: " + waveTimer);
        Debug.Log("spawn timer: " + spawnTimer);
        if (waveTimer <= 0 && spawnTimer <= 0) {
            DestroyExistingObjects();

            SpawnBlueTargets(currentWave);
            SpawnOrangeTargets(currentWave);
            currentWave++;
            waveTimer = timeBetweenWaves[currentWave];
            spawnTimer = waveTimer + 1;
        } else if (waveTimer <= 0) {
            DestroyExistingObjects();
            InstantiateSpawnEffects(currentWave);
        }
    }

    void InstantiateSpawnEffects(int waveNum) {
        foreach(Transform blueSpawnPt in blueSpawnPoints) {
            Destroy(Instantiate(spawnBlueTargetEffect, blueSpawnPt.position, Quaternion.identity), objectSpawnTime[waveNum]);
        }
        foreach(Transform orangeSpawnPt in orangeSpawnPoints) {
            Destroy(Instantiate(spawnOrangeTargetEffect, orangeSpawnPt.position, Quaternion.identity), objectSpawnTime[waveNum]);
        }
        waveTimer = objectSpawnTime[waveNum];
        spawnTimer = objectSpawnTime[waveNum];
    }

    void SpawnBlueTargets(int waveNum) {
        foreach (Transform blueSpawnPt in blueSpawnPoints) {
            activeBlueObjects.Add((GameObject) Instantiate(blueTargetModel, blueSpawnPt.position, Quaternion.identity));
        }
    }

    void SpawnOrangeTargets(int waveNum) {
        foreach (Transform orangeSpawnPt in orangeSpawnPoints) {
            GameObject targetGO = (GameObject) Instantiate(orangeTargetModel, orangeSpawnPt.position, Quaternion.identity);
            activeOrangeObjects.Add(targetGO);
        }
    }

    void DestroyExistingObjects() {
        foreach (GameObject blueTarget in activeBlueObjects) {
            if (blueTarget != null) {
                Destroy(Instantiate(despawnBlueTargetEffect, blueTarget.transform.position, Quaternion.identity), despawnBlueEffectTime);
                Destroy(blueTarget);
            }
        }
        activeBlueObjects = new List<GameObject>();

        foreach (GameObject orangeTarget in activeOrangeObjects) {
            if (orangeTarget != null) {
                Destroy(Instantiate(despawnOrangeTargetEffect, orangeTarget.transform.position, Quaternion.identity), despawnOrangeEffectTime);
                Destroy(orangeTarget);
            }
        }
        activeOrangeObjects = new List<GameObject>();
    }
}
