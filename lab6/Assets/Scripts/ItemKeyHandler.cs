using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemKeyHandler : MonoBehaviour
{

    private UIEventSubscriber eventSubscriber;
    private Transform itemPanel;
    private int slotCount;

    public GameObject inventoryHolder;
    private Inventory inventory;

    private InventoryItemClickable clickable;

    // Start is called before the first frame update
    void Start()
    {
        eventSubscriber = gameObject.GetComponent<UIEventSubscriber>();
        itemPanel = gameObject.transform.Find("ItemPanel").transform;
        slotCount = itemPanel.childCount;

        inventory = inventoryHolder.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++ )
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                eventSubscriber.selectedUIItem = itemPanel.GetChild(i).gameObject;
                eventSubscriber.selectedUIItem.GetComponent<InventoryItemClickable>().OnItemSwitched();
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (eventSubscriber.selectedUIItem != null)
            {
                clickable = eventSubscriber.selectedUIItem.GetComponent<InventoryItemClickable>();
                clickable.OnItemClicked();
            }
        }
    }
}
