using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [HideInInspector]
    public LevelDataManager levelData;
    public Transform target;

    public Transform maxX;
    public Transform minX;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {

        Vector3 v = maxX.position - minX.position;
        Vector3 target_position = minX.position + Random.value * v;

        GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemyInstance.GetComponent<BasicEnemyController>().destination = target_position;
        enemyInstance.GetComponent<BasicEnemyController>().targetPosition = target;
    }

    IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(levelData.spawnRate);

        foreach (EnemyData enemyData in levelData.enemyList)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                SpawnEnemy(enemyData.enemyPrefab);
                yield return wait;
            }
        }
    }
}
