using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBlade : MonoBehaviour
{
    public Weapon sword;
    public Dictionary<Weapon, float> destroyedWeapons = new Dictionary<Weapon, float>();
    public float activationTime = 3.0f;

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Sword Hit");
        Weapon w = other.gameObject.GetComponent<Weapon>();
        if(w != null && w.playerName != sword.playerName) {
            destroyedWeapons.Add(w, activationTime);
           w.DestroyWeapon();
        }
    }

    private void Update() {
        foreach(Weapon w in destroyedWeapons.Keys) {
            destroyedWeapons[w] = destroyedWeapons[w] - Time.deltaTime;
            if(destroyedWeapons[w] < 0) {
                w.EnableWeapon();
                destroyedWeapons.Remove(w);
            }
        }
    }
}
