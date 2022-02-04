using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerLeftDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerLeftDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        Debug.Log("Left danger: " + enemy.inDangerLeft());
        if (enemy.inDangerLeft())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
