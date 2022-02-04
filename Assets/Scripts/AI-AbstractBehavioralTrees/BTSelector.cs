using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTComposite
{
    private int currNode = 0;

    public BTSelector(BehaviorTree t, BTNode[] children) : base(t, children) {

    }

    public override Response Execute() {
        if (currNode < Children.Count) {
            Response response = Children[currNode].Execute();

            if (response == Response.Running) {
                return Response.Running;
            } else if (response == Response.Success) {
                currNode = 0;
                return Response.Success;
            } else {
                currNode++;
                if (currNode < Children.Count) {
                    return Response.Running;
                } else {
                    currNode = 0;
                    return Response.Failure;
                }
            }
        }

        return Response.Failure;
    }
}
