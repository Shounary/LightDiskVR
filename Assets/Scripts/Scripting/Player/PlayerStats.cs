using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int startHealth;
    public int health;
    public string playerName;

    public HealthBar healthBar;

    private void Start() {
        health = startHealth;
    }

    public void takeDamage(int damage) {
        health -= damage;
        if(health < 0)
            health = 0;
        healthBar.displayHealth(health);
        if (health == 0) 
            PauseController.instance.DeathMenu();
    }
}
