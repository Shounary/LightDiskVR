using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerStats : MonoBehaviour
{
    public int startHealth;
    public int health;
    public string playerName;
    public bool invincible = false; //set to true to make the player not take damage

    public HealthBar healthBar;
    public float timeSinceHit = 10.0f;
    public float invincibilityTime = 2.0f;
    public WeaponInventory weaponInventory;

    public GameObject endGameMenu;

    const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";

    private void Awake() {
        playerName = generateRandomName();
        weaponInventory.playerName = playerName;
    }

     public string generateRandomName()
    {
        string s = "";
        for(int i=0; i<10; i++)
        {
            s += glyphs[Random.Range(0, glyphs.Length)];
        }
        return s;

    }

    protected void Start() {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        timeSinceHit += Time.deltaTime;
    }

    public virtual void takeDamage(int damage) {
        if (timeSinceHit >= invincibilityTime ) { // timer so you can't take damage multiple times in 2 seconds (like if the disk passes through multiple hitboxes)
            timeSinceHit = 0.0f;
            if(!invincible)
            {
                health = Mathf.Max(0, health - damage);
            }
        }

        healthBar.displayHealth(health);

        if (health <= 0) {
            if(TutorialManager.instance == null)
            {
                endGameMenu.SetActive(true);
            }
            else
                TutorialManager.instance.onPlayerDeath();
        }
    }
}
