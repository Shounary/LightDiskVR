using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSideDodgeAction : BTNode
{
    public Enemy enemy;

    public LeftSideDodgeAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
            if (clip.name == "LeftSideDodge") {
                enemy.animator.speed = 1.5f;
                enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                Debug.Log(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
            }
        }

        enemy.animator.SetBool("isLeftSideDodging", true);
        return Response.Success;
    }
}
