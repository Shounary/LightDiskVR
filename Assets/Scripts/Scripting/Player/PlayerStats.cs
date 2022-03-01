using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int startHealth;
    public static float health;



    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public static void takeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            PauseController.instance.DeathMenu();
        }
    }
}
