using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private int score = 0;
    public int maxScore = 10;
    public int roundTime = 60;

    public Transform spawnPositions;

    public GameObject player;

    public Canvas HUD;

    public Text timerText;
    public Text loseText;
    public Text winText;
    public Text statsText;

    private enum RoundOverState { Win, Lose };
    private RoundOverState roundOver;

    private float timerFloat = 0.0f;
    private int timeSeconds = 0;

    private float shotsFired = 0;
    private float shotsHit = 0;
    private float accuracy = 0;

    void Awake()
    {
        Transform spawn = spawnPositions.GetChild(Random.Range(0, spawnPositions.childCount));
        Vector3 pos = spawn.position;
        pos.y += 2.0f;
        player.transform.position = pos;
        player.transform.rotation = spawn.rotation;

        //RoundOverState roundOverState;
    }

    // Update is called once per frame
    void Update()
    {
        timerFloat += Time.deltaTime;
        timeSeconds = (int)timerFloat % 60;
        timerText.text = "Time Remaining: " + (roundTime - timeSeconds);

        if (score == maxScore)
        {
            roundOver = RoundOverState.Win;
            EndRound();
        }
        if (roundTime - timeSeconds <= 0)
        {
            roundOver = RoundOverState.Lose;
            EndRound();
        }
    }

    void OnValidate()
    {
        // Validate variables
        score = 0;
        if (maxScore < 0) maxScore = 1;
    }

    void EndRound()
    {
        switch(roundOver)
        {
            case RoundOverState.Win:
                {
                    winText.gameObject.SetActive(true);
                    Debug.Log("Round Over: Win!");
                    break;
                }
            case RoundOverState.Lose:
                {
                    loseText.gameObject.SetActive(true);
                    Debug.Log("Round Over: Lose!");
                    break;
                }
        }
    }


    public void UpdateScore(int increment)
    {
        score += increment;
    }

    public void UpdateScore()
    {
        score += 1;
    }

    public void UpdateShotCount()
    {
        shotsFired += 1;
        UpdateStatsText();
    }

    public void UpdateHits()
    {
        shotsHit += 1;
        UpdateStatsText();
    }

    void UpdateStatsText()
    {

        if (shotsFired == 0)
        {
            accuracy = 0.0f;
        }
        else
        {
            float shotsMissed = shotsFired - shotsHit;
            //accuracy = (((float)shotsHit) - shotsMissed) / ((float)shotsFired);
            accuracy = ((float)shotsHit) / ((float)shotsFired);

            Debug.Log("shotsMissed: " + shotsMissed + " shotsFired: " + shotsFired + " shotsHit" + shotsHit);
        }
        statsText.text = "Shots Fired: " + shotsFired + "\n" + "Shots Hit: " + shotsHit + "\n" + "Accuracy: " + accuracy * 100.0f + "%";
    }

    void OnDrawGizmos()
    {
        if (spawnPositions == null) return;
        if (spawnPositions.GetChild(0) == null) return;

        foreach (Transform spawnPoint in spawnPositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(spawnPoint.position, .3f);

        }
    }
}
