using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSideDodgeAction : BTNode
{
    public Enemy enemy;

    public RightSideDodgeAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
            if (clip.name == "RightSideDodge") {
                enemy.animator.speed = 1.5f;
                enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                Debug.Log(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
            }
        }

        enemy.animator.SetBool("isRightSideDodging", true);
        return Response.Success;
    }

}
