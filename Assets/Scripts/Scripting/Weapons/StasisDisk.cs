using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisDisk : Weapon
{
    public int maxStasis = 1; //the number of times stasis can be used before 
    private int stasisCount = 0;

    public float velocityDivisor = 5;
    private Vector3 storedVelocity;
    private bool inStasis;
    //private bool isHeld;
    public ParticleSystem stasisStartParticles;
    public ParticleSystem stasisEndParticles;
    

    // Start is called before the first frame update
    void Start()
    {
        // Set up Stasis checks and physics stuff
        inStasis = false;
        isHeld = false;
    }

    public override void TriggerPressFunction()
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
        stasisStartParticles.Play();
    }

    private void endStasis() {
        inStasis = false;
        weaponRB.velocity = storedVelocity;
        weaponRB.isKinematic = false;
        stasisEndParticles.Play();
    }

    public override void OnReleaseFunction(int h)
    {
        base.OnReleaseFunction(h);
        stasisCount = 0;
    }
}
