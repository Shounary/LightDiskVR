using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerLowDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerLowDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        //Debug.Log(enemy.inDangerLow());
        if (enemy.inDangerLow())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
