using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public PlayerStats ps;
    //public float damageTakePerDiskHit = 100;
    public List<LayerMask> toTakeDamageFrom;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if(w != null && w.playerName != ps.playerName && toTakeDamageFrom.Contains((LayerMask) other.gameObject.layer))
            ps.takeDamage(w.damage);
    }

}
