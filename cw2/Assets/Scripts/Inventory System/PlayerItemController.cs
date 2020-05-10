using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles inventory management using player input
/// </summary>
public class PlayerItemController : MonoBehaviour
{

    [Header("Inventory References")]
    public Inventory inventory;

    [Header("Prefab Objects")]
    public GameObject bulletPrefab;

    // Currently displayed preview object 
    // For turrets, this is a "ghost" version of the turret
    // For weapons, this is the weapon model shown in the player's view
    private GameObject previewObject;

    // Time since last Timed Update
    private float deltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        if (inventory != null)
        {
            inventory.ItemSwitchEvent += OnItemSwitched;
            inventory.ItemUseEvent += OnItemUsed;
        }

        // Get the current item and trigger a switch event
        // This "selects" the first item when the game starts
        if (inventory.list[0] != null)
        {
            inventory.selectedItem = inventory.list[0];
            inventory.SwitchItem();
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Update time
        deltaTime += Time.deltaTime;

        // This loop gets the current input on the number row
        // Looping from 0 -> inventory count
        // I check the current input against KeyCode.Alpha1 (#1 on the number row)
        // This allows for a dynamic input behaviour, where only slots with selectable items can be used
        for (int i = 0; i < inventory.list.Count; i++)
        {
            // Compare actual input against keycodes
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (inventory.list[i].enabled)
                {  
                    // Switch to the selected item
                    inventory.selectedItem = inventory.list[i];
                    inventory.SwitchItem();
                }
            }
        }

        // On mouse click, use the currently selected item
        if (Input.GetButtonDown("Fire1"))
        {
            inventory.UseItem();
        }
   
    }
    
    // FixedUpdate is called 0/1/multiple times per frame
    // Used for making the preview object motion smoother
    private void FixedUpdate()
    {

        if (previewObject != null)
        {
            // First, Disable the preview object (in case it can't be shown anyway)
            previewObject.SetActive(false);

            // Get the current item type and use the appropriate preview method
            switch (inventory.selectedItem.itemType)
            {
                case (ItemType.Turret):
                    {
                        // Shows the preview object in front of the player on the ground
                        ShowTurretPreview();
                        break;
                    }
                case (ItemType.Weapon):
                    {
                        // shows the weapon model in the player's "hand"
                        previewObject.SetActive(true);
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Displays the preview object as a turret preview.
    /// This means that it uses the Playercontroller.placePosition as the preview object's transform position 
    /// </summary>
    void ShowTurretPreview()
    {
        if (PlayerController.Instance.turretPlaceable)
        {
            // Check if the player can build a turret
            bool canBuild = (GameController.Instance.achievedScore >= GameController.Instance.usedScore + Inventory.Instance.selectedItem.useCost);
            if (canBuild)
            {
                // Display the preview object
                previewObject.SetActive(true);
                previewObject.transform.position = PlayerController.Instance.placePosition;
                previewObject.transform.rotation = PlayerController.Instance.transform.rotation;

            }
        }
    }

    /// <summary>
    /// Event listener for ItemSwitch events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">InventoryEventArgs of the event</param>
    public void OnItemSwitched(object sender, InventoryEventArgs e)
    {
        ItemData item = e.item;

        // Destroy the current preview object
        if (previewObject != null) Destroy(previewObject);

        // Create the new item's preview object
        previewObject = Instantiate(item.previewObject);

        // If the preview object is a weapon
        // Set it's transform to be in the player's "hand"
        if (item.itemType == ItemType.Weapon)
        {
            previewObject.transform.SetParent(PlayerController.Instance.weaponTransform);
            previewObject.transform.localPosition = Vector3.zero;
            previewObject.transform.rotation = PlayerController.Instance.weaponTransform.rotation;
        }
    }

    /// <summary>
    /// Event listener for ItemUsed events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">InventoryEventArgs of the event</param>
    public void OnItemUsed(object sender, InventoryEventArgs e)
    {
        // Turret type is handled by a different script
        // Instantiate bullets, rate-limited at the fire rate of the weapon (given by ItemData.useCost)
        if (e.item.itemType == ItemType.Weapon)
        {
            // Check if last bullet was fired longer ago than the fire rate 
            if (deltaTime >= ((float)e.item.useCost)/1000.0f)
            {
                // Reset duration
                deltaTime = 0;

                // Instantiate bullet and set layer mask
                GameObject bullet = Instantiate(bulletPrefab, PlayerController.Instance.shotTransform.position, PlayerController.Instance.shotTransform.rotation);
                bullet.GetComponent<BulletController>().parentLayerMask = PlayerController.Instance.gameObject.layer;
            }
        }
    }
}
