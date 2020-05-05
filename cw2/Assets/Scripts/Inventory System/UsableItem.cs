using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public virtual bool UseItem(int useCost)
    {
        print("Item Used for " + useCost);
        return false;
    }
}
