using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private void OnCollisionEnter(Collision other) {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if(w != null)
            w.DestroyWeapon();
    }
}
