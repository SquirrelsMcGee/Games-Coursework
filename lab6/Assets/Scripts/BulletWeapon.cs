using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : MonoBehaviour
{

    [Header("Object References")]
    public GameObject inventoryHolder;
    public GameObject projectileTemplate;
    public Transform shotTransform;

    [Header("Weapon Settings")]
    public float fireRate;
    public float projectileSize;

    private Inventory inventory;
    private float deltaTime = 0;

    public void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();
        inventory.ItemFired += ItemFired;
    }
    public void Update()
    {
        deltaTime += Time.deltaTime;
        //Debug.Log(deltaTime);
    }

    void ItemFired(object sender, InventoryEventArgs e)
    {
        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            Debug.Log("Firing shot");
            Instantiate(projectileTemplate, shotTransform.position, Camera.main.transform.rotation);

        }
    }
}
