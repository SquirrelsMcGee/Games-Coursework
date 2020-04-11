using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    string itemName { get; }
    Sprite itemImage { get; }
    void onPickup();
}