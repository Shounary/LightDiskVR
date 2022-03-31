using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public TutorialSegment segment; //the current segment
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this is horrible and awful and I hate it, but I can't think of anything much better 
    public virtual void OnSegmentStart(TutorialSegment seg)
    {
        segment = seg;
        switch(seg.segmentID) {
            case 102:
                //Debug.Log("lol");
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
