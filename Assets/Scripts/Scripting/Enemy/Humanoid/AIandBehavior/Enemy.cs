using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Parameters")]
    public float threatPredictionTime = 1f;
    public float recoveryTimeOffset = 0f;
    public float sprintRecovery = 0.4f;
    public float leftRightDangerSensitivity = 1.3f;

    [Header("Variable References")]
    public Transform target;
    public Rigidbody[] diskThreats;
    public Transform enemyZone;

    [Header("Internal References")]
    public EnemyDiskInteractor enemyDiskInteractor;
    public Animator animator;
    public Transform rightLegPoint;
    public Transform leftLegPoint;
    public Transform centerPoint;
    public Transform headPoint;


    private float recoveryTimer;
    private bool wasReset;
    //private Vector3 currentDestination;

    private void Awake() {
        recoveryTimer = 0f;
        wasReset = true;
        //currentDestination = enemyZone.position;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (recoveryTimer > 0)
            recoveryTimer -= Time.deltaTime;

        if (!wasReset && recoveryTimer <= 0) {
            MainReset();
        }

        if (inZone()) {
            ReadyReset();
        }

    }

    private void OnAnimatorMove() {
        transform.position = animator.rootPosition;
    }

    // -------------- Daemon Functions ----------------
    public bool inDanger() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 direction = centerPoint.position - disk.position;
            if (Vector3.Distance(disk.position, centerPoint.position) < threatPredictionTime * Vector3.Dot(disk.velocity, direction.normalized)) {
                //&& Vector3.Dot(disk.velocity, direction.normalized) > 1.25f
                return true;
            }
        }
        return false;
    }

    public bool inDangerLow() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 highDirection = headPoint.position - disk.position;
            Vector3 middleDirection = centerPoint.position - disk.position;
            Vector3 lowDirection = (rightLegPoint.position + leftLegPoint.position) / 2 - disk.position;
            if (Vector3.Distance(disk.position, (rightLegPoint.position + leftLegPoint.position) / 2) < threatPredictionTime * Vector3.Dot(disk.velocity, lowDirection.normalized)) {
                return Vector3.Dot(disk.velocity, lowDirection.normalized) > Vector3.Dot(disk.velocity, highDirection.normalized) 
                    && Vector3.Dot(disk.velocity, lowDirection.normalized) > Vector3.Dot(disk.velocity, middleDirection.normalized);
            }
        }
        return false;
    }

    public bool inDangerMiddle() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 highDirection = headPoint.position - disk.position;
            Vector3 middleDirection = centerPoint.position - disk.position;
            Vector3 lowDirection = (rightLegPoint.position + leftLegPoint.position) / 2 - disk.position;
            if (Vector3.Distance(disk.position, centerPoint.position) < threatPredictionTime * Vector3.Dot(disk.velocity, middleDirection.normalized)) {
                return Vector3.Dot(disk.velocity, middleDirection.normalized) > Vector3.Dot(disk.velocity, highDirection.normalized)
                    && Vector3.Dot(disk.velocity, middleDirection.normalized) > Vector3.Dot(disk.velocity, lowDirection.normalized);
            }
        }
        return false;
    }

    public bool inDangerHigh() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 highDirection = headPoint.position - disk.position;
            Vector3 middleDirection = centerPoint.position - disk.position;
            Vector3 lowDirection = (rightLegPoint.position + leftLegPoint.position) / 2 - disk.position;
            if (Vector3.Distance(disk.position, headPoint.position) < threatPredictionTime * Vector3.Dot(disk.velocity, highDirection.normalized)) {
                return Vector3.Dot(disk.velocity, highDirection.normalized) > Vector3.Dot(disk.velocity, lowDirection.normalized)
                    && Vector3.Dot(disk.velocity, highDirection.normalized) > Vector3.Dot(disk.velocity, middleDirection.normalized);
            }
        }
        return false;
    }

    public bool inDangerRight() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 rightShift = transform.right * leftRightDangerSensitivity;
            Vector3 leftShift = -1 * transform.right * leftRightDangerSensitivity;
            Vector3 rightPoint = centerPoint.position + rightShift;
            Vector3 leftPoint = centerPoint.position + leftShift;
            Vector3 rightDirection = rightPoint - disk.position;
            Vector3 leftDirection = leftPoint - disk.position;
            if (Vector3.Distance(disk.position, rightPoint) < threatPredictionTime * Vector3.Dot(disk.velocity, rightDirection.normalized)) {
                return Vector3.Dot(disk.velocity, rightDirection.normalized) >= Vector3.Dot(disk.velocity, leftDirection.normalized);
            }
        }
        return false;
    }

    public bool inDangerLeft() {
        foreach (Rigidbody disk in diskThreats) {
            Vector3 rightShift = transform.right * leftRightDangerSensitivity;
            Vector3 leftShift = -1 * transform.right * leftRightDangerSensitivity;
            Vector3 rightPoint = centerPoint.position + rightShift;
            Vector3 leftPoint = centerPoint.position + leftShift;
            Vector3 rightDirection = rightPoint - disk.position;
            Vector3 leftDirection = leftPoint - disk.position;
            if (Vector3.Distance(disk.position, leftPoint) < threatPredictionTime * Vector3.Dot(disk.velocity, leftDirection.normalized)) {
                return Vector3.Dot(disk.velocity, rightDirection.normalized) < Vector3.Dot(disk.velocity, leftDirection.normalized);
            }
        }
        return false;
    }

    public bool inRecovery() {
        return recoveryTimer > 0;
    }

    // -------- Communication with Actions --------
    public void goInRecovery(float recoveryTime) {
        wasReset = false;
        animator.SetBool("inRecovery", true);
        recoveryTimer = recoveryTime;
    }

    // --------------- Pathfinding ----------------

    public bool inZone() {
        return Mathf.Abs(transform.position.x - enemyZone.position.x) <= enemyZone.localScale.x * 0.5f && Mathf.Abs(transform.position.z - enemyZone.position.z) <= enemyZone.localScale.z * 0.5f;
    }

    public Vector3 calculateDestinationVector() {
        Debug.Log("V3 magnitude Calculated: " + Vector3.Magnitude(enemyZone.position - transform.position));
        return enemyZone.position - transform.position;
    }






    // ------------------ Resets ------------------
    public void MainReset() {
        wasReset = true;
        animator.SetBool("inRecovery", false);
        animator.SetBool("isDownDodging", false);
        animator.SetBool("isJumpDodging", false);
        animator.SetBool("isLeftSideDodging", false);
        animator.SetBool("isRightSideDodging", false);
        animator.speed = 1f;
    }

    public void ReadyReset() {
        animator.SetBool("isMoving", false);
        //transform.LookAt(target);
    }





















    // Getters/Setters
    public float GetRecoveryTimer() {
        return recoveryTimer;
    }

    public void SetRecoveryTimer(float newTime) {
        recoveryTimer = newTime;
    }
}
