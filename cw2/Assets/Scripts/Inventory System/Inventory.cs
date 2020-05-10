using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Used to interface with the <c>InventoryDataManger</c> ScriptableObject
/// <para>Provides a set of methods for interacting with the inventory data</para>
/// </summary>
public class Inventory : MonoBehaviour
{
    /*
     * Public Variables 
    */
    [Header("InventoryData ScriptableObject Reference")]
    public InventoryDataManager inventoryData;

    [HideInInspector]
    public List<ItemData> list; // Public list of inventory items
    [HideInInspector]
    public ItemData selectedItem; // Public reference to the currently select inventory item

    [Header("Script Settings", order = 1)]
    [Tooltip("Toggles whether items get removed after a number of uses")]
    public bool enableItemDuration = false;
    
    // Inventory events
    public event EventHandler<InventoryEventArgs> ItemAddEvent;
    public event EventHandler<InventoryEventArgs> ItemRemoveEvent;
    public event EventHandler<InventoryEventArgs> ItemSwitchEvent;
    public event EventHandler<InventoryEventArgs> ItemUseEvent;

    public static Inventory Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); }

        // We don't do a Deep-copy of the InventoryData, so we need to reset any changed values on Start
        inventoryData.ResetAllCreated(); // This removes the changes

        // Create a copy of the list from the InventoryDataManager object
        list = new List<ItemData>(inventoryData.inventoryItems);
        
    }

    /// <summary>
    /// Event for when items are added to the inventory
    /// </summary>
    public void AddItem()
    {
        /* (Unused)
         * Inventory is predefined using Scriptable Objects
         * Current game does not have the player pick up items
        */

        // Broadcast event
        if (ItemAddEvent != null)
        {
            ItemRemoveEvent.Invoke(this, new InventoryEventArgs(InventoryEventType.Add, selectedItem));
        }
    }

    /// <summary>
    /// Event for when items are removed from the inventory (Unused)
    /// </summary>
    public void RemoveItem()
    {
        // Disable Item (Don't delete it because it may be useful later)
        selectedItem.enabled = false;

        // Broadcast event
        if (ItemRemoveEvent != null)
        {
            ItemRemoveEvent.Invoke(this, new InventoryEventArgs(InventoryEventType.Remove, selectedItem));
        }
    }

    /// <summary>
    /// Event for when the currently held item is switched
    /// </summary>
    public void SwitchItem()
    {
        // Broadcast event
        if (ItemSwitchEvent != null)
        {
            ItemSwitchEvent.Invoke(this, new InventoryEventArgs(InventoryEventType.Switch, selectedItem));
        }
    }

    /// <summary>
    /// Event for when the player uses the held item
    /// </summary>
    public void UseItem()
    {
        bool itemUsedUp; // Check if the item is "used up" / has no more uses
        
        // If the item type is a Turret
        GameObject instantiatedItem;
        
        if (selectedItem.itemType == ItemType.Turret)
        {
            // Instantiate the turret
            // This Utilises "SpawnTurret" prefabs
            // These are empty GameObjects with the SpawnTurret script attached
            // They instantiate the turret, and are destroyed after
            instantiatedItem = Instantiate(selectedItem.usableObject);
            Destroy(instantiatedItem);
        }

        // Broadcast event
        if (ItemUseEvent != null)
        {
            ItemUseEvent.Invoke(this, new InventoryEventArgs(InventoryEventType.Use, selectedItem));
        }

        // If the item duration is 0, remove the item
        itemUsedUp = selectedItem.UseItem();
        if (itemUsedUp && enableItemDuration)
        {
            RemoveItem();
        }
    }
}

/// <summary>
/// Stores information used by Inventory-related events
/// </summary>
public class InventoryEventArgs : EventArgs
{
    public InventoryEventType type; // Type of the inventory event
    public ItemData item; // Reference to the item data

    // Constructor
    public InventoryEventArgs(InventoryEventType type, ItemData item)
    {
        this.type = type;
        this.item = item;
    }
}

/// <summary>
/// Enum of different inventory event types
/// </summary>
public enum InventoryEventType
{
    Add,
    Remove,
    Switch,
    Use
}