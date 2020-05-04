using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public InventoryDataManager inventory;
    public GameObject slotPrefab;

    public Transform inventoryPanel;

    public Image healthUIBackground;
    public TextMeshProUGUI towerHealthUI;

    // Start is called before the first frame update
    void Start()
    {
        ClearHotBar();
        PopulateHotBar();

        inventory.ItemSwitchEvent += OnItemSwitched;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        towerHealthUI.text = TargetController.Instance.health + "/" + TargetController.Instance.maxHealth;
        float healthRatio = ((float)TargetController.Instance.health) / ((float)TargetController.Instance.maxHealth);
        Color temp = Color.Lerp(Color.red, Color.green, healthRatio);
        temp.a = 0.5f;
        healthUIBackground.color = temp;
    }

    void PopulateHotBar()
    {
        ClearHotBar();

        ItemData item;
        GameObject slot;

        for (int itemIndex = 0; itemIndex < inventory.inventoryItems.Count; itemIndex++)
        {
            slot = Instantiate(slotPrefab, inventoryPanel);

            item = inventory.inventoryItems[itemIndex];
            print(item == null);
            if (item.previewImage != null)
            {
                slot.GetComponent<Image>().enabled = true;
            }
            else
            {
                TextMeshProUGUI text = slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = item.itemName;
            }
        }
    }

    void ClearHotBar()
    {
        foreach (Transform slot in inventoryPanel)
        {
            Destroy(slot.gameObject);
        }
    }

    void OnItemSwitched(object sender, InventoryEventArgs e)
    {
        for (int itemIndex = 0; itemIndex < inventory.inventoryItems.Count; itemIndex++)
        {
            GameObject itemSlot = inventoryPanel.GetChild(itemIndex).gameObject;
            if (itemSlot != null)
            {
                itemSlot.GetComponent<Outline>().enabled = (inventory.inventoryItems[itemIndex] == e.item);
            }
        }
    }
}
