using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
    This is the main class used when making a tutorial. The tutorial manager stores
    a list of tutorial segments, objects which are used across multiple tutorial segments, 
    and GUI objects. It also has references to the controllers 
*/


public class TutorialManager : MonoBehaviour
{
    //stores a list of completion conditions, which are updated every frame
    //button com

    public Dictionary<string, bool> completionConditions = new Dictionary<string, bool>(); 


    List<TutorialSegment> segmentList = new List<TutorialSegment>(); //a list of all tutorial segments in order
    TutorialSegment currentSegment;

    public TextMeshProUGUI mainTutorialText;
    public TextMeshProUGUI highlightTutorialText;
    string currentClearCon;

    string tutorialLobbyScene;

    public static TutorialManager instance;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        completionConditions.Add("External", false); //used when completions are controlled by something else, such as UI
        completionConditions.Add("None", true); //used when a segment has no completion condition
        completionConditions.Add("Wait", false); //wait some alloted time
        completionConditions.Add("IsHeld", false); //is something being held?
    }

    // Update is called once per frame
    void Update()
    {
        //Get controller input





        //Move to next segment
        if(segmentList.Count == 0 && currentSegment == null) //tutorial complete
        {
            SceneManager.LoadScene(tutorialLobbyScene);
        }
        if(currentSegment == null) //if there is no active segment, move to the next one
        {
            StartNextSegment();
        }
        if(completionConditions[currentClearCon])
        {
            EndCurrentSegment();
        }
            
    }


    public void StartNextSegment() //gets and starts the next segment
    {
        currentSegment = segmentList[0];
        segmentList.RemoveAt(0);
        currentSegment.OnSegmentStart();
        //currentSegment.segmentDisplay.SetActive(true);
        currentClearCon = currentSegment.clearCon;
    }

    public void EndCurrentSegment() //ends the current segment
    {
        currentSegment.segmentDisplay.SetActive(false);
        if(currentClearCon.Equals("Wait"))
            completionConditions["Wait"] = false;
        currentClearCon = "";
        currentSegment = null;
    }

    public void OnGrabFunction()
    {
        completionConditions["IsHeld"] = true;
    }

    public void OnReleaseFunction()
    {
        completionConditions["IsHeld"] = false;
    }

    
}
