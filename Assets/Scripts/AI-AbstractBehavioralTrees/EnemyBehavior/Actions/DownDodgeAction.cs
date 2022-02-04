using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDodgeAction : BTNode
{
    public Enemy enemy;

    public DownDodgeAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
            if (clip.name == "DownDodge") {
                enemy.animator.speed = 1.5f;
                enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                Debug.Log(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
            }
        }
        enemy.animator.SetBool("isDownDodging", true);
        return Response.Success;
    }

}
