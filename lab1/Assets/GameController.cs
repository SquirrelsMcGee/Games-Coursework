using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float waveWait;

    int totalScore = 0;
    public void AddScore(int score)
    {
        totalScore = totalScore + score;
        Debug.Log("Current score:" + totalScore);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnValues.x, spawnValues.x),
                spawnValues.y,
               spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject asteroid = Instantiate(hazard, spawnPosition, spawnRotation);
                asteroid.GetComponent<Rigidbody>().angularVelocity =
                Random.insideUnitSphere * 10;
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}
