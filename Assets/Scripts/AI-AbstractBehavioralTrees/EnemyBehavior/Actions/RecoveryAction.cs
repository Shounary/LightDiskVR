using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryAction : BTNode
{
    public Enemy enemy;

    public RecoveryAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        return Response.Running;
    }
}
