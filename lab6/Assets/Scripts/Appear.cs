using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour
{

    bool playerInRange = false;

    public GameObject appearObject;
    public GameObject key;
    public GameObject inventoryHolder;
    private Inventory inventory;

    private void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();
        inventory.ItemRemoved += InventoryItemUsed;
        appearObject.SetActive(false);
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
        Debug.Log(appearObject.name + " is appearing");
        appearObject.SetActive(true);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
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
