using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public TrackedPoseDriver cameraDriver;
    public Rigidbody cameraRigidBody;
    public CapsuleCollider cameraPhysicsCollider;
    public GameObject deathMenuCanvas;
    public GameObject lRayInteractor;
    public GameObject rRayInteractor;
    public GameObject playerDeathEffect;
    public Transform deathEffectPoint;

    public static PauseController instance;

    private void Awake() {
        instance = this;
    }

    public void Pause() {
        
    }

    public void Continue() {

    }

    public void DeathMenu() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        cameraRigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        deathMenuCanvas.SetActive(true);
        cameraDriver.enabled = false;
        cameraRigidBody.isKinematic = false;
        lRayInteractor.SetActive(true);
        rRayInteractor.SetActive(true);
        cameraPhysicsCollider.enabled = true;
        StartCoroutine("FreezeGame");
        Destroy(Instantiate(playerDeathEffect, deathEffectPoint.position, deathEffectPoint.rotation), 2.0f);

    }

    IEnumerator FreezeGame() {
        for (float ft = 2f; ft > 0; ft -= 0.5f) {
            yield return new WaitForSeconds(.5f);
        }
        Time.timeScale = 0;
        yield break;
    }
}
