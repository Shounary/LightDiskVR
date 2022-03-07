using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int startHealth;
    public static int health;
    public string playerName;

    public HealthBar healthBar;
    private static float timeSinceHit = 10.0f;
    public static float invincibilityTime = 2.0f;


    private void Start() {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public void takeDamage(int damage) {
        if (timeSinceHit >= invincibilityTime) { // timer so you can't take damage multiple times in 2 seconds (like if the disk passes through multiple hitboxes)
            health -= damage;
            timeSinceHit = 0.0f;
            if(health <0)
                health = 0;
        }
        healthBar.displayHealth(health);
        if (health <= 0) {
            PauseController.instance.DeathMenu();
        }
    }
}
