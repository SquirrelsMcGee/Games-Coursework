using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{

    private GameObject previewTurret;
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (inventory.list[0] != null)
        {
            inventory.selectedItem = inventory.list[0];
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
        for (int i = 0; i < inventory.list.Count; i++)
        {
            //
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Debug.Log(KeyCode.Alpha1 + i);
                if (inventory.list[i].enabled)
                {  
                    inventory.selectedItem = inventory.list[i];
                    inventory.SwitchItem();
                }
            }
        }


        if (Input.GetButtonDown("Fire1"))
        {
            inventory.UseItem();
        }
    }

    private void FixedUpdate()
    {

        if (previewTurret != null)
        {

            previewTurret.SetActive(false);
            if (PlayerController.Instance.turretPlaceable)
            {
                if (GameController.Instance.achievedScore >= GameController.Instance.usedScore + inventory.selectedItem.useCost)
                {
                    previewTurret.SetActive(true);
                    previewTurret.transform.position = PlayerController.Instance.placePosition;
                    previewTurret.transform.rotation = PlayerController.Instance.transform.rotation;
                }
            }
        }
    }

    public void ItemSwitched(object sender, InventoryEventArgs e)
    {
        ItemData item = e.item;

        Destroy(previewTurret);

        if (item.previewObject != null)
        {
            previewTurret = Instantiate(item.previewObject);
        }
    }
}
