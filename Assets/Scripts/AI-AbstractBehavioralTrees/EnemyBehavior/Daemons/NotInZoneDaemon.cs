using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInZoneDaemon : BTDecorator
{
    public Enemy enemy;

    public NotInZoneDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (!enemy.inZone())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
