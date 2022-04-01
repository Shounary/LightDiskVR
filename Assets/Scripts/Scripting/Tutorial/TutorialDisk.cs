using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisk : TutorialObject
{
    public Weapon weapon;
    public int diskCollisions = 0;

    //this is horrible and awful and I hate it, but I can't think of anything much better 
    public override void OnSegmentStart(TutorialSegment seg)
    {
        segment = seg;
        switch(seg.segmentID) {
            case 102:
                weapon.isSummonable = false;
                weapon.EnableWeapon(seg.spawnPoint.position);
                break;
            case 104:
                weapon.isSummonable = true;
                break;
            case 107:
                weapon.DeactivateWeapon();
                break;
            case 109:
                weapon.EnableWeapon(seg.spawnPoint.position);
                break;

        }
    }
    private void Update() {
        if(segment.clearCon.Equals("Deflect") && diskCollisions >= 3) {
            TutorialManager.instance.completionConditions["Deflect"] = true;
            diskCollisions = 0;
        }
    }

    private void OnCollisionEnter(Collision other) {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if(w != null && other.gameObject.layer == 8)
            diskCollisions++;
    }
}
