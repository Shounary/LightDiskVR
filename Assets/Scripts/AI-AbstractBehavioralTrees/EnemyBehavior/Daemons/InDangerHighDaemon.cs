using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerHighDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerHighDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        //Debug.Log(enemy.inDangerHigh());
        if (enemy.inDangerHigh())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
