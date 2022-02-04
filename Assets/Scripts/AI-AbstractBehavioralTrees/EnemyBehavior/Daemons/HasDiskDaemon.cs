using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasDiskDaemon : BTDecorator
{
    public Enemy enemy;

    public HasDiskDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (enemy.enemyDiskInteractor.hasDisk)
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
