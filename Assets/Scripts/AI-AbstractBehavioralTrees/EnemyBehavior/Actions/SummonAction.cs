using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAction : BTNode
{
    public Enemy enemy;

    public SummonAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        if (!enemy.animator.GetBool("shouldSummon")) {
            return Response.Failure;
        }
        foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
            if (clip.name == "SummonStart") {
                enemy.animator.speed = 1.5f;
                enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset + 0.2f);
                //Debug.Log(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
            }
        }

        Vector3 distVector = enemy.enemyDiskInteractor.calculateDiskSummonVector();
        if (!enemy.animator.GetBool("isCatching") && !enemy.enemyDiskInteractor.hasDisk) {
            Vector2 distVector2D = new Vector2(distVector.x, distVector.z).normalized;
            Vector2 i = new Vector2(enemy.transform.right.x, enemy.transform.right.z);
            Vector2 j = new Vector2(enemy.transform.forward.x, enemy.transform.forward.z);
            Vector2 dir = new Vector2(Vector2.Dot(distVector2D, i), Vector2.Dot(distVector2D, j));
            enemy.animator.SetFloat("velx", dir.normalized.x);
            enemy.animator.SetFloat("vely", dir.normalized.y);
        }
        //Vector2 distVector2D = new Vector2(distVector.x, distVector.z).normalized;
        //Vector2 i = new Vector2(enemy.transform.right.x, enemy.transform.right.z);
        //Vector2 j = new Vector2(enemy.transform.forward.x, enemy.transform.forward.z);
        //Vector2 dir = new Vector2(Vector2.Dot(distVector2D, i), Vector2.Dot(distVector2D, j));
        //enemy.animator.SetFloat("velx", dir.normalized.x);
        //enemy.animator.SetFloat("vely", dir.normalized.y);





        enemy.animator.SetBool("isSummoning", true);
        return Response.Success;
    }
}
