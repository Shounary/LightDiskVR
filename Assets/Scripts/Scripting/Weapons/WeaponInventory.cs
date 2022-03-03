using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<Weapon> activeWeapons = new List<Weapon>(2); //stores the left hand weapon at index 0 and the right hand weapon at index 1
    public List<Weapon> weaponList; //a list of all weapons
    /*public HandActual leftHA;
    public HandActual rightHA;


    private void Update() {
        if(leftHA =)
    }*/

    private void Start() {
        activateWeapons();
    }

    public void addWeapon(Weapon weapon)
    {
        weaponList.Add(weapon);
    }

    public void activateWeapons()
    {
        activeWeapons[0] = weaponList[0];
        activeWeapons[0].hand = Hand.LEFT;
        activeWeapons[1] = weaponList[1];
        activeWeapons[1].hand = Hand.RIGHT;
        //this is good enough for now, as the player only has 2 active weapons
    }

    //returns the active weapon in the given hand
    public Weapon getActiveWeapon(Hand h)
    {
        return activeWeapons[(int) h];
    }

    //make w2 the active weapon of the hand for which w1 was previously the active weapon
    //returns if the swap was successful
    public bool swapActiveWeapon(Weapon replaceWeapon, Weapon newWeapon)
    {
        if(weaponList.Contains(newWeapon) && activeWeapons.Contains(replaceWeapon))
        {
            activeWeapons[activeWeapons.IndexOf(replaceWeapon)] = newWeapon;
            return true;
        }
        return false;
    }

    //places weapon w into the given hand
    //returns if the swap was successful
    public bool swapActiveWeapon(Hand h, Weapon w)
    {
        if(weaponList.Contains(w))
        {
            activeWeapons[(int) h] = w;
            return true;
        }    
        return false;
    }

}
