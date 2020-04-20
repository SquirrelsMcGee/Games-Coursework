using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemClickable : MonoBehaviour
{
    public IInventoryItem item;
    public GameObject inventoryHolder;
    private Inventory inventory;

    public void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();
    }

    public void Update()
    {
    }

    public void OnItemClicked()
    {
        if (item != null)
        {
            //Debug.Log("Using " + item.itemName);
            if (!item.isWeapon)
            {
                inventory.removeItem(item);
            }
            else {
                inventory.fireWeapon(item);
                //Debug.Log(item.root.name);
            }
        }
    }

    public void OnItemSwitched()
    {
        if (item != null)
        {
            inventory.switchItem(item);
        }
    }
}
