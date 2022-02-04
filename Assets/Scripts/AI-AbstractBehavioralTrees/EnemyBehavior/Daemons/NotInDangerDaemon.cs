using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInDangerDaemon : BTDecorator
{
    public Enemy enemy;

    public NotInDangerDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (!enemy.inDanger())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
