using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurvivalTracker : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI killScoreTMP;
    public TextMeshProUGUI timeScoreTMP;

    public int killScore;
    public float timeScore;

    // Start is called before the first frame update
    void Start()
    {
        killScore = 0;
        timeScore = 0f;
        killScoreTMP.text = "0";
        timeScoreTMP.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.health > 0) {
            timeScore += Time.deltaTime;
            UpdatePlayerTimeScore();
        }
    }

    public void IncrementKills() {
        if (playerStats.health > 0) {
            killScore++;
            UpdatePlayerKillScore();
        }
    }

    public void UpdatePlayerKillScore() {
        killScoreTMP.text = killScore.ToString();
    }

    public void UpdatePlayerTimeScore() {
        timeScoreTMP.text = System.Math.Round(timeScore, 1).ToString();
    }
}
