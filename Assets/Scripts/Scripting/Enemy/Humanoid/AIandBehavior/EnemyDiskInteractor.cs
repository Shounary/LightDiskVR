using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyDiskInteractor : MonoBehaviour
{
    [Header("Disk Summon Parameters")]
    public float timeBetweenAttacks = 2f;
    public float throwSpeedMagnitude = 8f;
    public float diskReturnForceMagnitude = 10f;
    public float stoppingFactorMultiplier = 0.2f;
    public float diskProximityThreshold = .35f;
    public List<Rigidbody> disks;

    [Header("Interanl References")]
    public Enemy enemy;
    public MultiAimConstraint head;
    public Animator animator;
    public Transform parentOfAttachPoint;
    public Transform attachPoint;
    public Transform target;

    [HideInInspector]
    public bool hasDisk;
    public bool isThrowingFrame;
    public float timeSinceLastAttack;
    public bool attackAlreadyReset;

    private void Start() {
        GetTargetDisk(transform).isKinematic = true;
        hasDisk = true;
        timeSinceLastAttack = 0f;
    }

    private void Update() {
        if (timeSinceLastAttack > 0) {
            timeSinceLastAttack -= Time.deltaTime;
        }
        if (timeSinceLastAttack <= 0) {
            animator.SetBool("shouldSummon", true);
        }

        if (hasDisk) {
            GetTargetDisk(transform).gameObject.transform.position = attachPoint.position;  // optimize
            GetTargetDisk(transform).gameObject.transform.rotation = attachPoint.rotation;
            GetTargetDisk(transform).velocity = Vector3.zero;
        }

        if (animator.GetBool("isSummoning") && !hasDisk && DiskIsCloseEnough(diskProximityThreshold)) {
            CatchDisk();
            animator.SetBool("isSummoning", false);
            foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
                if (clip.name == "Catch") {
                    enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                }
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SummonLoop")) {
            AttractDisk(1);
            enemy.goInRecovery(0.2f);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowEnd") && hasDisk) {
            ThrowDisk();
            animator.SetBool("shouldSummon", false);
            timeSinceLastAttack = timeBetweenAttacks;
            isThrowingFrame = false;
            animator.SetBool("isThrowing", false);
            foreach (AnimationClip clip in enemy.animator.runtimeAnimatorController.animationClips) {
                if (clip.name == "ThrowEnd") {
                    enemy.goInRecovery(clip.length / enemy.animator.speed + enemy.recoveryTimeOffset);
                }
            }
        }
    }

    public void AttractDisk(float additionalFactor) {
        Rigidbody targetDisk = GetTargetDisk(transform);
        Vector3 targetDirection = Vector3.Normalize(attachPoint.position - targetDisk.position);
        Vector3 initialDirection = Vector3.Normalize(targetDisk.velocity);
        float angle = Vector3.Angle(targetDirection, initialDirection);

        Vector3 normal = additionalFactor * stoppingFactorMultiplier * diskReturnForceMagnitude * Time.deltaTime * (-1) * Vector3.Magnitude(targetDisk.velocity) * Mathf.Abs(Mathf.Sin(Mathf.Abs(angle))) * initialDirection;
        Vector3 parallel = additionalFactor * diskReturnForceMagnitude * Time.deltaTime * targetDirection;

        if (angle > 5) {
            targetDisk.AddForce(normal, ForceMode.VelocityChange);
        }

        targetDisk.AddForce(parallel, ForceMode.VelocityChange);
    }

    public void CatchDisk() {
        hasDisk = true;
        Rigidbody targetDisk = GetTargetDisk(transform);
        targetDisk.velocity = Vector3.zero;
        targetDisk.isKinematic = true;
        //targetDisk.transform.parent = parentOfAttachPoint;
        targetDisk.gameObject.transform.position = attachPoint.position;
        targetDisk.gameObject.transform.rotation = attachPoint.rotation;
        targetDisk.transform.parent = parentOfAttachPoint;
    }

    public void ThrowDisk() {
        hasDisk = false;
        Rigidbody targetDisk = GetTargetDisk(transform);
        targetDisk.transform.parent = null;
        //transform.LookAt(target);
        targetDisk.velocity = calculateDiskThrowVector().normalized * throwSpeedMagnitude;
        GetTargetDisk(transform).isKinematic = false;
    }




    public bool DiskIsCloseEnough(float threshold) {
        return Vector3.Magnitude(attachPoint.position - GetTargetDisk(transform).position) < threshold;
    }

    public Vector3 calculateDiskSummonVector() {
        return GetTargetDisk(transform).position - transform.position;
    }

    public Vector3 calculateDiskThrowVector() {
        return target.position - attachPoint.position;
    }


    public Vector3 calculateBodyRotationThrowVector() {
        return target.position - transform.position;
    }

    private Rigidbody GetTargetDisk(Transform controllerTransform) {
        return disks[0]; // BETTER version to be implemented
    }


}
