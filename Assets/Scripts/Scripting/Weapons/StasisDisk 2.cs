using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StasisDisk : Weapon
{
    // This stuff probably needs to be changed to make compatible with more than one player
    // Sorry Chase
    
    public int maxStasis = 1;
    private int stasisCount = 0;

    public float velocityDivisor = 5;
    private Vector3 storedVelocity;
    private bool inStasis;
    //private bool isHeld;
    

    // Start is called before the first frame update
    void Start()
    {
        // Set up Stasis checks and physics stuff
        inStasis = false;
        isHeld = false;
    }

    public override void MainButtonFunction()
    {
        if (!inStasis && stasisCount < maxStasis && !isHeld) 
            startStasis();
        else if (inStasis)
            endStasis();
    }

    private void startStasis() {
        inStasis = true;
        storedVelocity = weaponRB.velocity;
        weaponRB.velocity = weaponRB.velocity / velocityDivisor;
        weaponRB.isKinematic = true;
        stasisCount++;
    }

    private void endStasis() {
        inStasis = false;
        weaponRB.velocity = storedVelocity;
        weaponRB.isKinematic = false;
    }

    public override void OnReleaseFunction()
    {
        base.OnReleaseFunction();
        stasisCount = 0;
    }
}
