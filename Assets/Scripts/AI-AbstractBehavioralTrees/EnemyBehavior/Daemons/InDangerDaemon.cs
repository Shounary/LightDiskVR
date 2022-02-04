using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (enemy.inDanger())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
