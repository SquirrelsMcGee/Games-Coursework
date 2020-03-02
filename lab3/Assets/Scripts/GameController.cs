using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int score = 0;
    public int maxScore { private set; get; } = 10;

    public GameObject[] spawnPositions;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        // Validate variables
        score = 0;
        if (maxScore < 0) maxScore = 1;
    }

}
