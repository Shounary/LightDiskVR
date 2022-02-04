using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInRecoveryDaemon : BTDecorator
{
    public Enemy enemy;

    public NotInRecoveryDaemon(BehaviorTree t, BTNode c, Enemy e) : base(t, c) {
        enemy = e;
    }

    public override Response Execute() {
        if (!enemy.inRecovery())
            return Child.Execute();

        Child.Reset();
        return Response.Failure;
    }
}
