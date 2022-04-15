using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalDroneKillIncrementor : MonoBehaviour
{
    public SurvivalTracker survivalTracker;
    
    // Start is called before the first frame update
    void Start()
    {
        survivalTracker = FindObjectOfType<SurvivalTracker>();
    }

    void OnTriggerEnter(Collider other) {
        survivalTracker.IncrementKills();
    }

    void OnCollisionEnter(Collision collision) {
        survivalTracker.IncrementKills();
    }
}
