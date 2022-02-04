using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2DAction : BTNode
{
    public Enemy enemy;

    public Move2DAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        Vector3 distVector = enemy.calculateDestinationVector();
        Vector2 distVector2D = new Vector2(distVector.x, distVector.z).normalized;
        Vector2 i = new Vector2(enemy.transform.right.x, enemy.transform.right.z);
        Vector2 j = new Vector2(enemy.transform.forward.x, enemy.transform.forward.z);
        Debug.Log("Forward: " + enemy.transform.forward);
        Debug.Log("Right: " + enemy.transform.right);

        Vector2 dir = new Vector2(Vector2.Dot(distVector2D, i), Vector2.Dot(distVector2D, j));

        enemy.goInRecovery(enemy.sprintRecovery);
        enemy.animator.SetBool("isMoving", true);

        enemy.animator.SetFloat("velx", dir.normalized.x);
        enemy.animator.SetFloat("vely", dir.normalized.y);

        Debug.Log("corresponding 2D vector: " + dir.normalized);

        return Response.Running;
    }
}
