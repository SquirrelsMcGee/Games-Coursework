using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public bool startOnLoad = true;

    [HideInInspector]
    public LevelDataManager levelData;
    public Transform target;

    public Transform maxX;
    public Transform minX;

    private bool inProgress = false;


    public static EnemySpawner Instance { get; private set; } // static singleton

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startOnLoad) StartLevel();
        GameController.Instance.GameEventLoss += OnGameLoss;
    }

    public void StartLevel()
    {
        Debug.Log("Starting" + levelData.name);
        StartCoroutine(SpawnLoop());
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {

        Vector3 v = maxX.position - minX.position;
        Vector3 destination = minX.position + Random.value * v;

        GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemyInstance.GetComponent<BasicEnemyController>().destination = destination;
        enemyInstance.GetComponent<BasicEnemyController>().targetPosition = target;
    }

    IEnumerator SpawnLoop()
    {
        inProgress = true;

        WaitForSeconds wait = new WaitForSeconds(levelData.spawnRate);

        foreach (EnemyData enemyData in levelData.enemyList)
        {
            if (!inProgress) break;
            for (int i = 0; i < enemyData.count; i++)
            {
                if (!inProgress) break;
                SpawnEnemy(enemyData.enemyPrefab);
                yield return wait;
            }
        }
        yield return null;
    }

    public void OnGameLoss(object sender, GameEventArgs e)
    {
        Debug.Log(e.state);
        if (e.state == GameStates.Loss)
        {
            inProgress = false;
        }
    }
}
