using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public PlayerStats ps;
    //public float damageTakePerDiskHit = 100;
    public LayerMask toTakeDamageFrom;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void OnTriggerEnter(Collider other) {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if(w != null && w.playerName != ps.playerName && other.gameObject.layer == 8)
            ps.takeDamage(w.damage);
    }

}
