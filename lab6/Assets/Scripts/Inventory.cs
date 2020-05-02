using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update

    List<IInventoryItem> items = new List<IInventoryItem>();

    public IInventoryItem selectedItem;
   
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    public event EventHandler<InventoryEventArgs> ItemFired;
    public event EventHandler<InventoryEventArgs> ItemSwitched;

    public void addItem(IInventoryItem item)
    {
        // Add item to inventory
        items.Add(item);

        item.onPickup();

        if (ItemAdded != null)
        {
            Debug.Log("Inventory addItem");
            ItemAdded.Invoke(this, new InventoryEventArgs(item));
        }
    }

    public void removeItem(IInventoryItem item)
    {
        // Use/Remove item from inventory
        if (ItemRemoved != null && item.isWeapon == false)
        {
            items.Remove(item);
            ItemRemoved.Invoke(this, new InventoryEventArgs(item));
        }
    }

    public void switchItem(IInventoryItem item)
    {
        // Switch selected item
        if (ItemSwitched != null)
        {
            ItemSwitched.Invoke(this, new InventoryEventArgs(item));
        }
    }

    public void fireWeapon(IInventoryItem item)
    {
        if (ItemFired != null)
        {
            ItemFired.Invoke(this, new InventoryEventArgs(item));
        }
    }
}

