using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerBody : MonoBehaviour
{
    public PlayerStats ps;
    //public float damageTakePerDiskHit = 100;
    public List<LayerMask> toTakeDamageFrom;
    public List<int> intMaskList = new List<int>();
    void Start()
    {
        foreach(LayerMask lm in toTakeDamageFrom) {
            intMaskList.Add(lm.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected async virtual void OnTriggerEnter(Collider other) {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if (w != null && w.playerName != ps.playerName && intMaskList.Contains(1 << other.gameObject.layer))
            ps.takeDamage(w.damage);
    }

}
