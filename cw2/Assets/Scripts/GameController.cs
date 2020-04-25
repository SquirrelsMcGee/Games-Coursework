using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class GameController : MonoBehaviour
{

    public LevelDataManager[] levelData;

    private EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.levelData = levelData[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
