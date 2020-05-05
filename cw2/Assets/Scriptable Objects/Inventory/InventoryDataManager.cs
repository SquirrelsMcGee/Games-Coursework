using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Inventory Data", menuName = "ScriptableObjects/InventoryData")]
public class InventoryDataManager : ScriptableObject
{
    public List<ItemData> inventoryItems;
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite previewImage;
    public GameObject previewObject;
    public GameObject usableObject;

    public ItemType itemType;

    public int useCost = 0;

    public bool enabled;

    public bool UseItem()
    {
        if (usableObject != null)
        {
            return usableObject.GetComponent<UsableItem>().UseItem(useCost);
        }
        return false;
    }
}

public enum ItemType
{
    Turret,
    Weapon
}
