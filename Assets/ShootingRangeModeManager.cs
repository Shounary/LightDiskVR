using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootingRangeModeManager : MonoBehaviour
{
    public TextMeshProUGUI playerScoreTMP;
    public TextMeshProUGUI playerTimeTMP;
    public int playerScore = 0;
    public float playerTimer = 20.0f;

    public static ShootingRangeModeManager instance;

    void Awake() {
        instance = this;
    }

    void Update()
    {
        if (playerTimer > 0)
            playerTimer -= Time.deltaTime;
        else
            playerTimer = 0;
        UpdatePlayerTimer();
    }

    public void increaseScoreIfGameNotOver(int playerScore) {
        if (playerTimer > 0) {
            this.playerScore += playerScore;
            UpdatePlayerTextScore();
        }            
    }

    public void UpdatePlayerTextScore() {
        playerScoreTMP.text = playerScore.ToString();
    }

    public void UpdatePlayerTimer() {
        playerTimeTMP.text = System.Math.Round(playerTimer, 1).ToString();
    }
}
