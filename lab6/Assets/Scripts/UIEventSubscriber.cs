using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventSubscriber : MonoBehaviour
{

    [Header("Object References")]
    public GameObject inventoryHolder;
    private Inventory inventory;

    [HideInInspector]
    public GameObject selectedUIItem;

    private Transform panel;
    // Start is called before the first frame update
    void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();
        
        // Subscribe to events
        inventory.ItemAdded += ItemAdded;
        inventory.ItemSwitched += ItemSwitched;
        inventory.ItemRemoved += ItemUsed;

        panel = transform.Find("ItemPanel");

        selectedUIItem = null;

        // Hide all HUD slots at start
        foreach (Transform slot in panel)
        {
            Image image = slot.GetComponent<Image>();

            image.enabled = false;
        }
    }

    public void ItemAdded(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.item;

        // Enable the correct slot for the item
        
        foreach (Transform slot in panel)
        {
            Image image = slot.GetComponent<Image>();
            InventoryItemClickable button = slot.GetComponent<InventoryItemClickable>();
            
            slot.GetComponent<Outline>().enabled = false;

            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = item.itemImage;
                slot.GetComponent<Outline>().enabled = true;
                button.item = e.item;
                selectedUIItem = slot.gameObject;
                break;
            }
        }
    }

    public void ItemSwitched(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.item;
        Debug.Log(item.itemName);
        foreach (Transform slot in panel)
        {
            slot.GetComponent<Outline>().enabled = (slot.gameObject == selectedUIItem);
        }
    }

    void ItemUsed(object sender, InventoryEventArgs e)
    {
        foreach (Transform slot in panel)
        {

            InventoryItemClickable button = slot.GetComponent<InventoryItemClickable>();

            if (button.item == e.item)
            {
                Image image = slot.GetComponent<Image>();

                if (image.enabled)
                {
                    image.enabled = false;
                    image.sprite = null;
                    slot.GetComponent<Outline>().enabled = false;
                    button.item = null;
                    selectedUIItem = null;
                    break;
                }
            }
        }
    }

    public void OnDestroy()
    {
        inventory.ItemAdded -= ItemAdded;
        inventory.ItemSwitched -= ItemSwitched;
        inventory.ItemRemoved -= ItemUsed;
    }
}
