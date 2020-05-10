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

    // Used for displaying the game end state
    public GameObject winPanel;
    public GameObject lossPanel;

    [Header("Prefab Objects", order = 1)]
    public GameObject slotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ClearHotBar();
        PopulateHotBar();

        inventory.ItemSwitchEvent += OnItemSwitched;
        inventory.ItemUseEvent += OnItemUsed;

        if (inventory.list[0] != null)
        {
            inventoryPanel.GetChild(0).GetComponent<Outline>().enabled = true;
        }

        inventory.SwitchItem();

        GameController.Instance.GameEventLoss += OnGameLoss;
        GameController.Instance.GameEventWin += OnGameWin;


        winPanel.SetActive(false);
        lossPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    /// <summary>
    /// Updates the on screen UI relating to the defense point's current health
    /// </summary>
    void UpdateHealthUI()
    {
        towerHealthUI.text = TargetController.Instance.health + "/" + TargetController.Instance.maxHealth;
        float healthRatio = ((float)TargetController.Instance.health) / ((float)TargetController.Instance.maxHealth);
        Color temp = Color.Lerp(Color.red, Color.green, healthRatio);
        temp.a = 0.5f;
        healthUIBackground.color = temp;
    }


    /// <summary>
    /// Populates the inventory selection bar with the currently loaded inventory list
    /// </summary>
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

    /// <summary>
    /// Clears the inventory selection bar
    /// </summary>
    void ClearHotBar()
    {
        foreach (Transform slot in inventoryPanel)
        {
            Destroy(slot.gameObject);
        }
    }

    /// <summary>
    /// Event listener for ItemSwitch events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">InventoryEventArgs of the event</param>
    void OnItemSwitched(object sender, InventoryEventArgs e)
    {
        UpdateItemInfo(e.item);
    }


    /// <summary>
    /// Event listener for ItemUsed events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">InventoryEventArgs of the event</param>
    void OnItemUsed(object sender, InventoryEventArgs e)
    {
        UpdateItemInfo(e.item);
    }

    /// <summary>
    /// Updates the on screen info box for the currently selected item
    /// </summary>
    /// <param name="item">ItemData of the currently selected item</param>
    void UpdateItemInfo(ItemData item)
    {
        // Loops through the list of items
        for (int itemIndex = 0; itemIndex < inventory.list.Count; itemIndex++)
        {
            GameObject itemSlot = inventoryPanel.GetChild(itemIndex).gameObject;
            if (itemSlot != null)
            {
                // Enables the outline component for the currently selected item, disables all others
                itemSlot.GetComponent<Outline>().enabled = (inventory.list[itemIndex] == item);
            }
        }

        // Initialises the info text
        infoText.text = item.itemName + "\n";


        // Set text appropriately for each item
        if (item.itemType == ItemType.Turret)
        {
            int _useCost = item.useCost + item.GetCreated();
            infoText.text += "Build cost: " + _useCost + "\n";
            infoText.text += "Can Build: " + (GameController.Instance.achievedScore >= GameController.Instance.usedScore + item.useCost).ToString();
        }
        else
        {
            // if ItemType.Weapon
            infoText.text += "Fire Delay: " + item.useCost + "ms";
        }
    }

    /// <summary>
    /// Event listener for the GameEventWin event
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e">GameEventArgs for the event</param>
    public void OnGameWin(object s, GameEventArgs e)
    {
        // Show Game Win Screen
        StartCoroutine(DelayedEndGame(winPanel));
    }

    /// <summary>
    /// Event listener for the GameEventLoss event
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e">GameEventArgs for the event</param>
    public void OnGameLoss(object s, GameEventArgs e)
    {
        // Show Game Loss Screen
        StartCoroutine(DelayedEndGame(lossPanel));
    }

    /// <summary>
    /// Enables the given panel to show either the Win or Loss panel at the end of a round
    /// </summary>
    /// <param name="endPanel"></param>
    /// <returns></returns>
    IEnumerator DelayedEndGame(GameObject endPanel)
    {

        // Show end panel
        EnemySpawner.Instance.enabled = false;
        endPanel.SetActive(true);

        yield return new WaitForSeconds(5);

        // Load main menu after 5 seconds
        SceneLoader sceneLoader = new SceneLoader { sceneId = 0 };
        sceneLoader.LoadScene();
    }
}
