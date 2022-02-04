using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerRightDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerRightDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        Debug.Log("RIght danger: " + enemy.inDangerRight());
        if (enemy.inDangerRight())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
