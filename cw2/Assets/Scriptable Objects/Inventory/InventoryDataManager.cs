using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ScriptableObject used for storing multiple ItemData objects.
/// </summary>
[CreateAssetMenu(fileName = "New Inventory Data", menuName = "ScriptableObjects/InventoryData")]
public class InventoryDataManager : ScriptableObject
{
    // List of ItemData obejcts
    public List<ItemData> inventoryItems;

    // Resets all ItemData.created to 0
    // Inventory class does not perform a deep copy of the ItemData objects, so the ScriptableObject is updated when it shouldn't
    // Small workaround to solve this issue
    public void ResetAllCreated()
    {
        foreach (ItemData item in inventoryItems) item.ResetCreated();
    }
}


/// <summary>
/// Stores Item Data for use in the inventory system
/// </summary>
[System.Serializable]
public class ItemData
{
    // Name of the item
    public string itemName;

    // Preview image to be displayed
    public Sprite previewImage;

    // Preview object to be displayed
    // For weapons, this is the weapon model shown in the first person view
    public GameObject previewObject;

    // Object to be instantiated on click
    // For turrets, this is the SpawnTurret prefabs
    public GameObject usableObject;

    // Type of the item (Turret / Weapon)
    public ItemType itemType;

    // Cost to use
    // For turrets, this is the number of enemies that need to be killed before a new turret can be created
    // For weapons, this is the number of milliseconds to wait before the player can fire again
    public int useCost = 0;

    // Flag for hiding items
    public bool enabled;

    // Number of turrets that have been created
    private int _created = 0;

    // Increments the number of created turrets by 1
    public void IncrementCreated()
    {
        _created += 1;
    }

    // Returns the number of created turrets
    public int GetCreated()
    {
        return _created;
    }

    // Sets the number of created turrets to 0
    public void ResetCreated()
    {
        _created = 0;
    }

    // Calls the UsableItem.UseItem method
    public bool UseItem()
    {
        if (usableObject != null)
        {
            return usableObject.GetComponent<UsableItem>().UseItem(useCost);
        }
        return false;
    }
}

/// <summary>
/// Type of the Item
/// </summary>
public enum ItemType
{
    Turret,
    Weapon
}
