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

    // Start is called before the first frame update
    void Start()
    {
        completionConditions.Add("External", false); //used when completions are controlled by something else, such as UI
        completionConditions.Add("None", true); //used when a segment has no completion condition
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
        if(completionConditions[currentClearCon] || (currentSegment.useWaitTime && currentSegment.waitTime < 0.0f))
        {
            EndCurrentSegment();
        }
            
    }


    public void StartNextSegment() //gets and starts the next segment
    {
        currentSegment = segmentList[0];
        segmentList.RemoveAt(0);
        currentSegment.segmentDisplay.SetActive(true);
       // mainTutorialText.text = currentSegment.mainText;
        //highlightTutorialText.text = currentSegment.highlightText;
        currentClearCon = currentSegment.clearCon;
    }

    public void EndCurrentSegment() //ends the current segment
    {
        //mainTutorialText.text = "";
       // highlightTutorialText.text = "";
        currentSegment.segmentDisplay.SetActive(false);
        currentClearCon = "";
        currentSegment = null;
    }

    
}
