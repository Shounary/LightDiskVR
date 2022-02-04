using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTComposite : BTNode
{
    public List<BTNode> Children { get; set; }
    
    public BTComposite(BehaviorTree t, BTNode[] nodes) : base(t) {
        Children = new List<BTNode>(nodes);
    }
}
