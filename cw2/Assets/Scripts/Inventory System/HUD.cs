using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    /*
     * Public Variables 
    */

    [Header("Inventory References", order = 0)]
    public Inventory inventory;
    public Transform inventoryPanel;

    [Header("UI References", order = 1)]
    // Used for displaying tower helath
    public Image healthUIBackground;
    public TextMeshProUGUI towerHealthUI;

    // Used for displaying item information to the player
    public TextMeshProUGUI infoText;
    public GameObject infoPanel;

    [Header("Prefab Objects", order = 1)]
    public GameObject slotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ClearHotBar();
        PopulateHotBar();

        inventory.ItemSwitchEvent += OnItemSwitched;

        if (inventory.list[0] != null)
        {
            inventoryPanel.GetChild(0).GetComponent<Outline>().enabled = true;
        }

        inventory.SwitchItem();
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

        for (int itemIndex = 0; itemIndex < inventory.list.Count; itemIndex++)
        {
            slot = Instantiate(slotPrefab, inventoryPanel);

            item = inventory.list[itemIndex];
            //print(item.previewImage == null);
            if (item.previewImage != null)
            {
                slot.GetComponent<Image>().enabled = true;
                slot.GetComponent<Image>().sprite = item.previewImage;
                slot.transform.GetChild(0).gameObject.SetActive(false); // Hide text if showing image :)
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
        for (int itemIndex = 0; itemIndex < inventory.list.Count; itemIndex++)
        {
            GameObject itemSlot = inventoryPanel.GetChild(itemIndex).gameObject;
            if (itemSlot != null)
            {
                itemSlot.GetComponent<Outline>().enabled = (inventory.list[itemIndex] == e.item);
            }
        }

        infoText.text = e.item.itemName + "\n";
        if (e.item.itemType == ItemType.Turret)
        {
            infoText.text += "Build cost: " + e.item.useCost + "\n";
            infoText.text += "Can Build: "  + (GameController.Instance.achievedScore >= GameController.Instance.usedScore + e.item.useCost).ToString();
        } else {
            infoText.text += "Fire Delay: " + e.item.useCost + "ms";
        }
    }
}
