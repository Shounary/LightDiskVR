using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode
{
    public enum Response {Running, Failure, Success};
    public BehaviorTree BehaviorTree { get; set; }

    public BTNode(BehaviorTree t) {
        BehaviorTree = t;
    }

    public virtual Response Execute() {
        return Response.Failure;
    }

    public virtual void Reset() {

    }
}
