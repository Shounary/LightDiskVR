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
    
    public void setType(int type) {
        variations[type].SetActive(true);
        this.type = type;
    }

    public void updateStats() {
        if (type == 2)
            scoreText.text = ShootingRangeModeManager.instance.playerScore.ToString();
    }

    private void OnEnable() {
        if(ShootingRangeModeManager.instance) {
            setType(2);
            updateStats();  
        }  
        else {
            setType(ps.health <= 0? 0: 1);
        }
    }

}
