using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    string itemName { get; }
    Sprite itemImage { get; }
    void onPickup();

    float fireRate { get; }

    bool isWeapon { get; }

    GameObject model { get; set; }
    GameObject root { get; set; }
    BulletWeapon weapon { get; set; }
}

public class InventoryEventArgs : EventArgs
{
    public IInventoryItem item;
    public InventoryEventArgs(IInventoryItem item)
    {
        this.item = item;
    }
}