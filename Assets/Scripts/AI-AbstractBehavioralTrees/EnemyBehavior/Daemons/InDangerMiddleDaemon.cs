using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDangerMiddleDaemon : BTDecorator
{
    public Enemy enemy;
    public InDangerMiddleDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (enemy.inDangerMiddle())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
