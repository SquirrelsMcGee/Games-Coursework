using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class GameController : MonoBehaviour
{
    public LevelDataManager[] levelData;
    private int currentLevel = 0;
    private int totalLevels = 0;

    private EnemySpawner enemySpawner;

    public event EventHandler<GameEventArgs> GameEventEnd;
    public event EventHandler<GameEventArgs> GameEventWin;
    public event EventHandler<GameEventArgs> GameEventLoss;

    [HideInInspector]
    public int requiredScore = 0;
    [HideInInspector]
    public int achievedScore = 0;
    [HideInInspector]
    public int usedScore = 0; // Score "used" purchasing turrets etc.

    public static GameController Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.levelData = levelData[0];
        requiredScore = levelData[0].totalEnemies;

        totalLevels = levelData.Length;
        currentLevel = 0;

        Debug.Log(totalLevels);

        TargetController.Instance.InitHealth(levelData[0].targetHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (achievedScore >= requiredScore)
        {
            achievedScore = 0;
            GameEnd(GameStates.Win);
        }
    }

    public void GameEnd(GameStates state)
    {

        Debug.Log("Game Ended!");

        if (GameEventEnd != null)
        {
            GameEventEnd.Invoke(this, new GameEventArgs(GameStates.End));
        }
        if (state == GameStates.Win)
        {
            Debug.Log("Game Win!");
            GameWin(state);
        }
        else if (state == GameStates.Loss)
        {
            Debug.Log("Game Loss!");
            GameLoss(state);
        }
    }

    public void GameWin(GameStates state)
    {
        if (GameEventWin != null)
        {
            GameEventWin.Invoke(this, new GameEventArgs(GameStates.Win));
        }

        // Load next level
        if (currentLevel < totalLevels - 1)
        {
            currentLevel += 1;

            enemySpawner.levelData = levelData[currentLevel];
            requiredScore = levelData[currentLevel].totalEnemies;

            enemySpawner.StartLevel();

            TargetController.Instance.InitHealth(levelData[currentLevel].targetHealth);
        }
    }

    public void GameLoss(GameStates state)
    {
        if (GameEventLoss != null)
        {
            GameEventLoss.Invoke(this, new GameEventArgs(GameStates.Loss));
        }
    }
}


public enum GameStates
{
    End,
    Win,
    Loss
}

public class GameEventArgs : EventArgs
{
    public GameStates state;

    public GameEventArgs(GameStates state) {
        this.state = state;
    }
}