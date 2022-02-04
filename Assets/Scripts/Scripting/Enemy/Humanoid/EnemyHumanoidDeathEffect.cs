using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHumanoidDeathEffect : MonoBehaviour
{
    public GameObject deathRH;
    public GameObject deathLH;
    public GameObject deathTorso;
    public GameObject deathRL;
    public GameObject deathLL;

    public Transform referensePointRH;
    public Transform referensePointLH;
    public Transform referensePointTorso;
    public Transform referensePointRL;
    public Transform referensePointLL;

    public LayerMask toTakeDamageFrom;
    private void OnTriggerEnter(Collider other) {
        Debug.Log("hit-----------------------------------------------------------------------------------------------------------------");
        if (toTakeDamageFrom == (toTakeDamageFrom | (1 << other.gameObject.layer))) {
            Destroy(Instantiate(deathRH, referensePointRH.position, referensePointRH.rotation), 2f);
            Destroy(Instantiate(deathLH, referensePointLH.position, referensePointLH.rotation), 2f);
            Destroy(Instantiate(deathTorso, referensePointTorso.position, referensePointTorso.rotation), 2f);
            Destroy(Instantiate(deathRL, referensePointRL.position, referensePointRL.rotation), 2f);
            Destroy(Instantiate(deathLL, referensePointLL.position, referensePointLL.rotation), 2f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("hit Oncollition-----------------------------------------------------------------------------------------------------------------");
        if (toTakeDamageFrom == (toTakeDamageFrom | (1 << collision.collider.gameObject.layer))) {
            Destroy(Instantiate(deathRH, referensePointRH.position, referensePointRH.rotation), 2f);
            Destroy(Instantiate(deathLH, referensePointLH.position, referensePointLH.rotation), 2f);
            Destroy(Instantiate(deathTorso, referensePointTorso.position, referensePointTorso.rotation), 2f);
            Destroy(Instantiate(deathRL, referensePointRL.position, referensePointRL.rotation), 2f);
            Destroy(Instantiate(deathLL, referensePointLL.position, referensePointLL.rotation), 2f);
            Destroy(gameObject);
        }
    }
}
