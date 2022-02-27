using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int startHealth;
    public static float health;

    private static float timeSinceHit = 10.0f;
    public static float invincibilityTime = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public static void takeDamage(float damage) {
        if (timeSinceHit >= invincibilityTime) { // timer so you can't take damage multiple times in 2 seconds (like if the disk passes through multiple hitboxes)
            health -= damage;
            timeSinceHit = 0.0f;
        }
        if (health <= 0) {
            PauseController.instance.DeathMenu();
        }
    }
}
