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
    public Transform spawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(useWaitTime && enabled)
            waitTime -= Time.deltaTime;
        if(waitTime <= 0.0)
        {
            TutorialManager.instance.completionConditions["Wait"] = true;
            useWaitTime = false;
            waitTime = 1.0f;
        }
        
    }

    //used for changing properties of objects which exist across multiple segments
    public void OnSegmentStart() {
        foreach(TutorialObject o in segmentUpdateObjects)
            o.OnSegmentStart(this);
        segmentDisplay.SetActive(true);
    }
}
