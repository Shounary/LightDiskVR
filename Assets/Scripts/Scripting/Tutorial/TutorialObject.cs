using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public TutorialSegment segment; //the current segment
    public List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnSegmentStart(TutorialSegment seg)
    {
        foreach (GameObject o in objects)
            o.SetActive(true);
    }
}
