using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    A tutorial segment is a small section of a tutorial. It can display texts and images,
    create and destory objects, and OTHER THING
    The tutorial segment has a completion condition, 
*/

public class TutorialSegment : MonoBehaviour
{
    //public string mainText;
    //public string highlightText;
    public int segmentID; //of the form Tutorial #, segment # (ie basic tutorial segment 8 = 108)
    public string clearCon;
    public bool useWaitTime; //
    public float waitTime; //the time to wait before advancing to the next segment.

    public GameObject segmentDisplay; // a display including text, images, gifs, etc
    public List<TutorialObject> segmentUpdateObjects;
    public List<GameObject> segmentDisableObjects;
    public Transform spawnPoint;

    public PlayerStats playerStats;
    public GameObject healthBar;
    public List<GameObject> clearList; //sets clear condition to true once this list is empty
    public TutorialSegment deathSegment; //the segment to go to after death


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) return;
        if(useWaitTime && enabled)
            waitTime -= Time.deltaTime;
        if(waitTime <= 0.0)
        {
            if (clearCon.Equals("Wait"))
                TutorialManager.instance.completionConditions["Wait"] = true;
            useWaitTime = false;
            waitTime = 1.0f;
        }
        for(int i = 0; i < clearList.Count; i++)
        {
            GameObject o = clearList[i];
            if(o == null){
                clearList.Remove(o);
                i--;
            }
        }
        if(clearCon.Equals("EmptyList") && clearList.Count == 0)
        {
            TutorialManager.instance.completionConditions["EmptyList"] = true;
        }
        if(segmentID == 108 && playerStats.timeSinceHit > 15.0f)
        {
            TutorialManager.instance.EndCurrentSegment();
        }
        
    }
  
    //used for changing properties of objects which exist across multiple segments
    public void OnSegmentStart() {
        foreach(TutorialObject o in segmentUpdateObjects)
            o.OnSegmentStart(this);
        if(segmentDisplay != null)
            segmentDisplay.SetActive(true);
        switch(segmentID) {
            case 108:
                playerStats.timeSinceHit = 0.0f;
                break;
            case 111:
                playerStats.invincible = false;
                healthBar.SetActive(true);
                break;
            case 113:
                healthBar.SetActive(true);
                playerStats.healthBar.displayHealth(playerStats.health);
                break;
            case 114:
                healthBar.SetActive(false);
                playerStats.health = 100;
                break;

        }
        foreach(GameObject o in segmentDisableObjects)
            o.SetActive(false);
    }
}
