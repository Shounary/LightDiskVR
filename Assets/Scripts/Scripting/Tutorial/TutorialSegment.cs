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
    public string mainText;
    public string highlightText;
    public string clearCon;

    public GameObject segmentDisplay; // a display including text, images, gifs, etc


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnSegmentStart() 
    {

    }

    public void OnSegmentEnd()
    {

    }
}
