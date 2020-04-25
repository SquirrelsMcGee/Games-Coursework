using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyData
{
    public GameObject enemyPrefab;
    public int count;
}


[CreateAssetMenu(fileName ="New Level Data", menuName ="ScriptableObjects/LevelData")]
public class LevelDataManager : ScriptableObject
{

    public int spawnRate = 1;
    public int targetHealth = 50;

    public float totalEnemies
    {
        get
        {
            return 0;
        }
        private set {
            totalEnemies = value;
        }
    }

    
    public EnemyData[] enemyList;
}
