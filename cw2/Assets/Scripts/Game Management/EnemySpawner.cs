using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies from EnemyDataManger objects
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public bool startOnLoad = true;

    [Range(0, 10)]
    public int spawnMultiplier = 1; // Multiplies the number of enemies spawned

    [Tooltip("Increase Enemy Health based on spawnMultiplier")]
    public bool modifyEnemyHealth = false;

    [HideInInspector]
    // The LevelData object, this is provided by GameController
    public LevelDataManager levelData;

    [Header("Scene References")]
    [Tooltip("Position of the defence point")]
    public Transform target;
    [Tooltip("Used for semi-random destination calculation")]
    public Transform maxX;
    [Tooltip("Used for semi-random destination calculation")]
    public Transform minX;

    [Tooltip("Array of spawn points to instantiate enemies at")]
    public Transform[] enemySpawnPoints;
    [Tooltip("Position to spawn boss enemies")]
    public Transform bossSpawnPoint;

    // Flag for disabling the spawn loop
    private bool inProgress = false;

    // Currently used spawn point
    private Transform spawnPoint;

    // Static singleton reference
    public static EnemySpawner Instance { get; private set; }

    void Awake()
    {
        // Create singleton reference
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start the level if true
        if (startOnLoad) StartLevel();

        // Subscribe to Game Loss Events
        GameController.Instance.GameEventLoss += OnGameLoss;
    }

    /// <summary>
    /// Starts the level
    /// </summary>
    public void StartLevel()
    {
        Debug.Log("Starting" + levelData.name);
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// Spawns enemies at a given rate
    /// </summary>
    IEnumerator SpawnLoop()
    {
        inProgress = true;

        // Waits for the duration specified
        WaitForSeconds wait = new WaitForSeconds(levelData.spawnRate);

        // Iterates over each EnemyData object in the list
        foreach (EnemyData enemyData in levelData.enemyList)
        {
            // Stops if set to false
            if (!inProgress) break;

            // Loops for the number of enemies specified
            for (int i = 0; i < enemyData.count; i++)
            {
                if (!inProgress) break;

                // Loops for the number defined by the spawnMultiplier
                for (int j = 0; j < spawnMultiplier; j++)
                {
                    // Gets a random spawn location
                    spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];

                    // If the enemy is a boss, set the spawn location to the boss spawn location
                    if (enemyData.isBoss)
                    {
                        if (bossSpawnPoint != null) spawnPoint = bossSpawnPoint;

                    }

                    // Spawn the enemy
                    SpawnEnemy(enemyData.enemyPrefab);

                    // Wait
                    yield return wait;
                }
            }
        }

        yield return null;
    }

    /// <summary>
    /// Instantiates an enemy from a given prefab object
    /// </summary>
    /// <param name="enemyPrefab">Enemy prefab to instantiate</param>
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Calculate destination position
        Vector3 v = maxX.position - minX.position;
        Vector3 destination = minX.position + Random.value * v;

        // Instantiate enemy from prefab at spawnpoint location
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Modifies enemy health using spawn multiplier
        if (modifyEnemyHealth)
        {
            enemyInstance.GetComponent<BasicEnemyController>().maxHealth += ((spawnMultiplier / 2) + 1);
            enemyInstance.GetComponent<BasicEnemyController>().InitHealth();
        }

        // Sets the enemy's navmesh agent destination
        enemyInstance.GetComponent<BasicEnemyController>().destination = destination;
        enemyInstance.GetComponent<BasicEnemyController>().targetPosition = target;
    }

    // Game loss event listener
    public void OnGameLoss(object sender, GameEventArgs e)
    {
        if (e.state == GameStates.Loss)
        {
            inProgress = false;
        }
    }


    /// <summary>
    /// Used for debugging spawning positions
    /// </summary>
    private void OnDrawGizmos()
    {
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            Gizmos.DrawSphere(spawnPoint.position, 0.3f);
        }

        if (bossSpawnPoint != null) Gizmos.DrawSphere(bossSpawnPoint.position, 0.3f);
    }
}
