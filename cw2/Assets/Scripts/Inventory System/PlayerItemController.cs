using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public InventoryDataManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        if (inventory.inventoryItems[0] != null)
        {
            inventory.selectedItem = inventory.inventoryItems[0];
        }

        if (inventory != null)
        {
            inventory.ItemSwitchEvent += ItemSwitched;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from number row
        for (int i = 0; i < inventory.inventoryItems.Count; i++)
        {
            //
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (inventory.inventoryItems[i].enabled)
                {  
                    inventory.selectedItem = inventory.inventoryItems[i];
                    inventory.SwitchItem();
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            inventory.UseItem();
        }
    }

    public void ItemSwitched(object sender, InventoryEventArgs e)
    {
        ItemData item = e.item;
        print(item.itemName);
    }
}
