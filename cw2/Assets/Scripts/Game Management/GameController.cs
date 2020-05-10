using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the game state. Including broadcasting events when the game is won/loss
/// </summary>
[RequireComponent(typeof(EnemySpawner))]
public class GameController : MonoBehaviour
{
    [Header("Object Data")]
    [Tooltip("Level Data to use")]
    // Scenes can support multiple level data, but the prototype only uses 1 per scene
    public LevelDataManager[] levelData;

    // Private variables for keeping track of the current level within the scene
    private int currentLevel = 0;
    private int totalLevels = 0;
    
    // Reference to the EnemySpawner script attached to the game object
    private EnemySpawner enemySpawner;

    // Events to broadcast on game end states
    public event EventHandler<GameEventArgs> GameEventEnd;
    public event EventHandler<GameEventArgs> GameEventWin;
    public event EventHandler<GameEventArgs> GameEventLoss;

    [HideInInspector]
    // The required score to defeat the level = the total number of enemies
    public int requiredScore = 0;
    [HideInInspector]
    // The currently achieved score
    public int achievedScore = 0;
    [HideInInspector]
    // The amount of score that has been "used" for building turrets
    public int usedScore = 0;

    // Static singleton reference
    public static GameController Instance { get; private set; }

    void Awake()
    {
        // Create singleton reference
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the EnemySpawner script and set its level data
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.levelData = levelData[0];

        // Calculate the required score to beat the level
        requiredScore = levelData[0].TotalEnemies * enemySpawner.spawnMultiplier;
        print("Total Enemies: " + levelData[0].TotalEnemies);
        print("Required Score: " + requiredScore);

        // Set the level indexing
        totalLevels = levelData.Length;
        currentLevel = 0;

        // Initialise the defence point health
        TargetController.Instance.InitHealth(levelData[0].targetHealth);
    }

    // Update is called once per frame
    void Update()
    {

        // Check if the player has won the game
        if (achievedScore >= requiredScore)
        {
            achievedScore = 0;
            GameEnd(GameStates.Win);
        }
    }

    /// <summary>
    /// Adds the given value to the achievedScore
    /// </summary>
    /// <param name="add">Value to add</param>
    public void AddScore(int add)
    {
        achievedScore += add;
    }

    /// <summary>
    /// Uses the given value, adds to the usedScore
    /// </summary>
    /// <param name="add">Value to use</param>
    public void UseScore(int add)
    {
        usedScore += add;
    }

    /// <summary>
    /// Function to call when the game should end
    /// </summary>
    /// <param name="state">Ending GameState (Win/Lose)</param>
    public void GameEnd(GameStates state)
    {

        Debug.Log("Game Ended!");

        // Broadcast generic end game event
        if (GameEventEnd != null)
        {
            GameEventEnd.Invoke(this, new GameEventArgs(GameStates.End));
        }

        // Broadcast game win event
        if (state == GameStates.Win)
        {
            Debug.Log("Game Win!");
            GameWin(state);
        }

        // Broadcast game loss event
        else if (state == GameStates.Loss)
        {
            Debug.Log("Game Loss!");
            GameLoss(state);
        }
    }

    /// <summary>
    /// Broadcasts the GameEventWin event
    /// </summary>
    /// <param name="state"></param>
    public void GameWin(GameStates state)
    {
        if (GameEventWin != null)
        {
            GameEventWin.Invoke(this, new GameEventArgs(GameStates.Win));
        }

        // Load next level (not used in prototype)
        if (currentLevel < totalLevels - 1)
        {
            currentLevel += 1;

            enemySpawner.levelData = levelData[currentLevel];
            requiredScore = levelData[currentLevel].TotalEnemies * enemySpawner.spawnMultiplier;

            enemySpawner.StartLevel();

            TargetController.Instance.InitHealth(levelData[currentLevel].targetHealth);
        }
    }

    /// <summary>
    /// Broadcasts the GameEventLoss event
    /// </summary>
    /// <param name="state"></param>
    public void GameLoss(GameStates state)
    {
        if (GameEventLoss != null)
        {
            GameEventLoss.Invoke(this, new GameEventArgs(GameStates.Loss));

            // Prevents any other events from firing
            achievedScore = 0;
            requiredScore = int.MaxValue;
        }
    }
}

/// <summary>
/// States the end game result
/// </summary>
public enum GameStates
{
    End,
    Win,
    Loss
}

/// <summary>
/// Wrapper class for event data
/// </summary>
public class GameEventArgs : EventArgs
{
    public GameStates state;

    public GameEventArgs(GameStates state) {
        this.state = state;
    }
}