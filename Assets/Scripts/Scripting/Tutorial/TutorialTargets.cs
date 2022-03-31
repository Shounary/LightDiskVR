using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargets : TutorialObject
{
    public GameObject targetParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnSegmentStart(TutorialSegment seg)
    {
        targetParent.SetActive(true);
    }
}
