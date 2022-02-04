using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public float damageTakePerDiskHit = 100;
    public LayerMask toTakeDamageFrom;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (toTakeDamageFrom == (toTakeDamageFrom | (1 << other.gameObject.layer))) {
            PlayerStats.takeDamage(damageTakePerDiskHit);
        }
    }

}
