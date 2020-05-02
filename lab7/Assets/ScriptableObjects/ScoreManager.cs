using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreManagerData", menuName = "ScriptableObjects/ScoreManager", order = 1)]
public class ScoreManager : ScriptableObject
{
    public float bestTime = 100.0f;

    public void setTime(float newTime, int trackNumber)
    {
        bestTime = Mathf.Min(bestTime, newTime);
    }
}
