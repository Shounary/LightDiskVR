using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitDisk : Weapon
{
    public Rigidbody diskRB;
    public Vector3 initSpeed;
    public GameObject topHalf;
    public GameObject bottomHalf;

    // Start is called before the first frame update
    void Start()
    {
        diskRB.velocity = initSpeed;

    }

    public override void MainButtonFunction()
    { 
        spawnDisk();
    }

    private void spawnDisk()
    {
        Destroy(diskRB);
        GameObject topPart = Instantiate(topHalf) as GameObject;
        GameObject bottomPart = Instantiate(bottomHalf) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
