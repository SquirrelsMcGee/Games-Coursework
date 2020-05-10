using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of the hint, if it is Timed, then the HUDHints loop will wait for the duration given in HintItem.time. If it is Wait, then it will wait for its waitHintFlag to be set to true
/// </summary>
public enum HintType
{
    Timed,
    Wait
}

/// <summary>
/// Stores Hint data to be used in the HUD hints system
/// </summary>
[System.Serializable]
public class HintItem
{
    [TextArea(0, 10)]
    // The text content of the hint
    public string hintText;

    // The type of the hint (Timed / Wait)
    public HintType type;

    // If the hint type is set to Timed, then this is the duration the HUDHints script will wait for
    public float time;

    // GameObject to instantiate when the Hint is first shown
    public GameObject appearObject;
}

/// <summary>
/// ScriptableObject used for storing multiple HintItem objects.
/// </summary>
[CreateAssetMenu(fileName = "New Hint Data", menuName = "ScriptableObjects/HintData")]
public class HintsDataManager : ScriptableObject
{
    public HintItem[] hints;
}
