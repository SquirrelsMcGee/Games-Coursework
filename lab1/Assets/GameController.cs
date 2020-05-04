using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    private int _hazards = 0;
    public float spawnWait;
    public float waveWait;

    public int waveCount = 1;
    [HideInInspector]
    public int waveIndex = 0;

    [Header("Scene References")]
    public Text waveText;
    public Text scoreText;

    public GameObject win;
    public GameObject lose;

    public static GameController Instance { get; private set; }

    int totalScore = 0;
    public void AddScore(int score)
    {
        totalScore = totalScore + score;
        scoreText.text = "Score: " + totalScore;
        Debug.Log("Current score:" + totalScore);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());

        if (Instance == null) Instance = this;
        else { Destroy(Instance.gameObject); }

        win.SetActive(false);
        lose.SetActive(false);
    }


    IEnumerator SpawnWaves()
    {
        while (waveIndex < waveCount)
        {
            waveText.text = "Wave:" + (waveIndex + 1);
            _hazards = hazardCount + (Random.Range(0, waveIndex + 5));
            for (int i = 0; i < _hazards; i++)
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
            waveIndex++;
        }
        GameController.Instance.win.SetActive(true);
        yield return new WaitForSeconds(2);
        
    }
}
