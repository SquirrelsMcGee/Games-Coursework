using UnityEngine;

/// <summary>
/// Stores enemy wave data for use in the enemy spawning system
/// </summary>
[System.Serializable]
public struct EnemyData
{
    public GameObject enemyPrefab;
    public int count;
    public bool isBoss;
}


/// <summary>
/// ScriptableObject used for storing multiple EnemyData objects.
/// </summary>
[CreateAssetMenu(fileName ="New Level Data", menuName ="ScriptableObjects/LevelData")]
public class LevelDataManager : ScriptableObject
{
    // How fast the enemies spawn
    public int spawnRate = 1;

    // How much health the defence point has
    public int targetHealth = 50;

    // Calculates the total number of enemies using EnemyData.count
    public int TotalEnemies
    {
        get
        {
            int temp = 0;
            foreach (EnemyData data in enemyList)
            {
                temp += data.count;
            }
            return temp;
        }
        private set {
            TotalEnemies = value;
        }
    }

    // Array of EnemyData objects
    public EnemyData[] enemyList;
}
