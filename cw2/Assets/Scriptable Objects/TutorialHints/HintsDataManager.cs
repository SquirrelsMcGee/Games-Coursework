using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HintType
{
    Timed,
    Wait
}

[System.Serializable]
public class HintItem
{
    [TextArea]
    public string hintText;
    public HintType type;

    public float time;

    public GameObject appearObject;
}

[CreateAssetMenu(fileName = "New Hint Data", menuName = "ScriptableObjects/HintData")]
public class HintsDataManager : ScriptableObject
{
    public HintItem[] hints;
}
