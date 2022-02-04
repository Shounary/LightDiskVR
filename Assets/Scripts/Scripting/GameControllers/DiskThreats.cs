using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskThreats : MonoBehaviour
{
    public static DiskThreats instance;

    public Rigidbody[] playerDisks;
    public Rigidbody[] enemyDisks;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
