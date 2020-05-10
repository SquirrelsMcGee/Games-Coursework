using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for usable item behaviours
/// </summary>
public class UsableItem : MonoBehaviour
{

    /// <summary>
    /// Base method for item usage. Returns true if the item duration is 0
    /// </summary>
    /// <param name="useCost">Cost of using the item</param>
    /// <returns>false</returns>
    public virtual bool UseItem(int useCost)
    {
        print("Item Used for " + useCost);
        return false;
    }
}
