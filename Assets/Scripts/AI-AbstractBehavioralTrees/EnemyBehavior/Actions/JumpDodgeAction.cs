using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDodgeAction : BTNode
{
    public Enemy enemy;

    public JumpDodgeAction(BehaviorTree t, Enemy e) : base(t) {
        enemy = e;
    }

    public override Response Execute() {
        if (enemy.animator.GetBool("isMoving")) {
            foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
                if (clip.name == "JumpDodge") {
                    enemy.animator.speed = 1.5f;
                    enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                }
            }
        } else {
            foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
                if (clip.name == "Backflip") {
                    enemy.animator.speed = 1.85f;
                    enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                }
            }
        }
        enemy.animator.SetBool("isJumpDodging", true);
        return Response.Success;
    }
}
