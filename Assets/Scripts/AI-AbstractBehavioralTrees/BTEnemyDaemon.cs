using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTEnemyDaemon : BTDecorator
{ 
    public BTEnemyDaemon(BehaviorTree t, BTNode c) : base(t, c) {

    }

    public override Response Execute() {
        return base.Execute();
    }
}
