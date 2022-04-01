using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisk : TutorialObject
{
    public Weapon weapon;

    //this is horrible and awful and I hate it, but I can't think of anything much better 
    public override void OnSegmentStart(TutorialSegment seg)
    {
        segment = seg;
        switch(seg.segmentID) {
            case 102:
                weapon.isSummonable = false;
                weapon.EnableWeapon(seg.spawnPoint);
                break;
            case 104:
                weapon.isSummonable = true;
                break;
            case 107:
                weapon.DeactivateWeapon();
                break;
            case 109:
                weapon.EnableWeapon(seg.spawnPoint);
                break;

        }
    }
}
