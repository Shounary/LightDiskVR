using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour
{
    public int pointReward;
    public GameObject destructionEffect;
    private void OnCollisionEnter(Collision collision) {
        ShootingRangeModeManager.instance.increaseScoreIfGameNotOver(pointReward);
        Destroy(Instantiate(destructionEffect, gameObject.transform.position, gameObject.transform.rotation), 2.2f);
        Destroy(gameObject);
    }
}
