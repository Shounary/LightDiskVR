using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameMenu : MonoBehaviour
{
    //parent gameobjects of features that change depending on the menu
    //index 0 = death, index 1 = victory, index 2 = score
    int type;
    public List<GameObject> variations = new List<GameObject>();
    public TextMeshProUGUI scoreText;
    public PlayerStats ps;

    public Rigidbody cameraRigidBody;
    public GameObject playerDeathEffect;
    public Transform deathEffectPoint;

    public bool isDead;
    public List<GameObject> disableOnDeath = new List<GameObject>();
    
    public void setType(int type) {
        if(type == 0)
            Die();
        if (type == 2)
            scoreText.text = ShootingRangeModeManager.instance.playerScore.ToString();
        variations[type].SetActive(true);
        this.type = type;
    }

    private void OnEnable() {
        if(ShootingRangeModeManager.instance) {
            setType(2);
            Time.timeScale = 0;
        }  
        else {
            setType(ps.health <= 0? 0: 1);
        }
    }

    public void Die() {
        cameraRigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        Destroy(Instantiate(playerDeathEffect, deathEffectPoint.position, deathEffectPoint.rotation), 2.0f);
        foreach(GameObject o in disableOnDeath)
            o.SetActive(false);
        isDead = true;
        Time.timeScale = 0;
    }

}
