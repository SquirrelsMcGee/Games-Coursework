using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{

    public InventoryDataManager inventoryData;

    public event EventHandler<InventoryEventArgs> ItemAddEvent;
    public event EventHandler<InventoryEventArgs> ItemRemoveEvent;
    public event EventHandler<InventoryEventArgs> ItemSwitchEvent;
    public event EventHandler<InventoryEventArgs> ItemUseEvent;

    public List<ItemData> list;


    [HideInInspector]
    public ItemData selectedItem;

    public void Awake()
    {
        list = new List<ItemData>(inventoryData.inventoryItems);
    }

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
        bool itemUsedUp;

        GameObject instantiatedItem;
        if (selectedItem.itemType == ItemType.Turret)
        {
            instantiatedItem = Instantiate(selectedItem.usableObject);
            Destroy(instantiatedItem);
        }

        itemUsedUp = selectedItem.UseItem();

        // Broadcast event
        if (ItemUseEvent != null)
        {
            ItemUseEvent.Invoke(this, new InventoryEventArgs(InventoryState.Use, selectedItem));
        }
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