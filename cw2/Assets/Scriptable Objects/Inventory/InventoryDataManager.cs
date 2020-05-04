using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Inventory Data", menuName = "ScriptableObjects/InventoryData")]
public class InventoryDataManager : ScriptableObject
{
    public List<ItemData> inventoryItems;

    [HideInInspector]
    public ItemData selectedItem;

    public event EventHandler<InventoryEventArgs> ItemAddEvent;
    public event EventHandler<InventoryEventArgs> ItemRemoveEvent;
    public event EventHandler<InventoryEventArgs> ItemSwitchEvent;
    public event EventHandler<InventoryEventArgs> ItemUseEvent;

    public void AddItem()
    {
        // Not sure about this one chief
        
        // Broadcast event
        if (ItemAddEvent != null)
        {
            ItemRemoveEvent.Invoke(this, new InventoryEventArgs(InventoryState.Add, selectedItem));
        }
    }

    public void RemoveItem()
    {
        // Disable Item
        selectedItem.enabled = false;

        // Broadcast event
        if (ItemRemoveEvent != null)
        {
            ItemRemoveEvent.Invoke(this, new InventoryEventArgs(InventoryState.Remove, selectedItem));
        }
    }

    public void SwitchItem()
    {
        
        // Broadcast event
        if (ItemSwitchEvent != null)
        {
            ItemSwitchEvent.Invoke(this, new InventoryEventArgs(InventoryState.Switch, selectedItem));
        }
    }

    public void UseItem()
    {
        selectedItem.UseItem();

        // Broadcast event
        if (ItemUseEvent != null)
        {
            ItemUseEvent.Invoke(this, new InventoryEventArgs(InventoryState.Use, selectedItem));
        }
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite previewImage;
    public GameObject previewObject;
    public GameObject usableObject;   

    public bool enabled;

    public void UseItem()
    {
        if (usableObject != null) usableObject.GetComponent<UsableItem>().UseItem();
    }
}

public class InventoryEventArgs : EventArgs
{
    public InventoryState state;
    public ItemData item;

    public InventoryEventArgs(InventoryState state, ItemData item)
    {
        this.state = state;
        this.item = item;
    }
}

public enum InventoryState
{
    Add,
    Remove,
    Switch,
    Use
}