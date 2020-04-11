using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour, IInventoryItem
{
    public Sprite _itemImage;
    public string _itemName;

    public string itemName
    {
        get
        {
            return _itemName;
        }
    }
    public Sprite itemImage
    {
        get
        {
            return _itemImage;
        }
    }
    public void onPickup()
    {
        gameObject.SetActive(false); // “picking up” merely makes it invisible
    }
}