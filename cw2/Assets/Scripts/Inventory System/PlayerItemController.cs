using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{

    private GameObject previewObject;
    public Inventory inventory;

    public GameObject bulletPrefab;

    private float deltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (inventory.list[0] != null)
        {
            inventory.selectedItem = inventory.list[0];
        }

        if (inventory != null)
        {
            inventory.ItemSwitchEvent += OnItemSwitched;
            inventory.ItemUseEvent += OnItemUsed;
        }
    }

    // Update is called once per frame
    void Update()
    {

        deltaTime += Time.deltaTime;

        // Get input from number row
        for (int i = 0; i < inventory.list.Count; i++)
        {
            //
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
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

        if (previewObject != null)
        {

            previewObject.SetActive(false);

            switch (inventory.selectedItem.itemType)
            {
                case (ItemType.Turret):
                    {
                        if (PlayerController.Instance.turretPlaceable)
                        {
                            if (GameController.Instance.achievedScore >= GameController.Instance.usedScore + inventory.selectedItem.useCost)
                            {
                                previewObject.SetActive(true);
                                previewObject.transform.position = PlayerController.Instance.placePosition;
                                previewObject.transform.rotation = PlayerController.Instance.transform.rotation;

                            }
                        }
                        break;
                    }
                case (ItemType.Weapon):
                    {

                        previewObject.SetActive(true);
                        break;
                    }
            }
        }
    }

    public void OnItemSwitched(object sender, InventoryEventArgs e)
    {
        ItemData item = e.item;

        Destroy(previewObject);

        if (item.previewObject != null)
        {
            previewObject = Instantiate(item.previewObject);
        }

        if (item.itemType == ItemType.Weapon)
        {
            previewObject.transform.SetParent(PlayerController.Instance.weaponTransform);
            previewObject.transform.localPosition = Vector3.zero;
            previewObject.transform.rotation = PlayerController.Instance.weaponTransform.rotation;
        }
    }

    public void OnItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.item.itemType == ItemType.Weapon)
        {
            if (deltaTime >= ((float)e.item.useCost)/1000.0f)
            {
                deltaTime = 0;
                GameObject bullet = Instantiate(bulletPrefab, PlayerController.Instance.shotTransform.position, PlayerController.Instance.shotTransform.rotation);
                bullet.GetComponent<BulletController>().parentLayerMask = PlayerController.Instance.gameObject.layer;
            }
        }
    }

    public void FireBullet()
    {

    }
}
