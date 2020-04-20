using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    bool playerInRange = false;

    public GameObject key;
    public GameObject inventoryHolder;
    private Inventory inventory;

    private void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();
        inventory.ItemRemoved += InventoryItemUsed;
    }

    void InventoryItemUsed(object sender, InventoryEventArgs e)
    {
        // check if correct item used
        if ((e.item as MonoBehaviour).gameObject == key)
        {
            // check if in range
            if (playerInRange)
            {
                Open();
            }
        }
    }

    void Open()
    {
        Debug.Log(gameObject.name + " is opening");
        gameObject.SetActive(false);
        playerInRange = false;
        Debug.Log("Player no longer in range of " + gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerInRange = true;
            Debug.Log("Player in range of " + gameObject.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerInRange = false;
            Debug.Log("Player no longer in range of " + gameObject.name);
        }
    }
}
