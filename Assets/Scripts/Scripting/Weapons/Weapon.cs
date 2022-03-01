using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    public Color primaryColor;
    public Color accentColor;
    public float damage;
    public bool isSummonable;
    public List<string> storableLocations;
    public string player;
    //public Collider hurtBox; 
    public Hand hand;

}
