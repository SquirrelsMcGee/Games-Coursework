using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoader : MonoBehaviour
{

    public ScoreManager scoreData1;
    public ScoreManager scoreData2;

    public Text scoreText1;
    public Text scoreText2;

    public float minRequirement = 60.0f;
    public Button lockedStage;

    // Start is called before the first frame update
    void Start()
    {
        string score1 = scoreData1.bestTime == 10000 ? "Unplayed" : scoreData1.bestTime.ToString("F2") + "s";
        string score2 = scoreData2.bestTime == 10000 ? "Unplayed" : scoreData2.bestTime.ToString("F2") + "s";

        scoreText1.text = "Best: " + score1;
        scoreText2.text = "Best: " + score2;

        // Enable second stage button if the first stage requirement was completed
        lockedStage.enabled = (scoreData1.bestTime <= minRequirement);
    }
}
