using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public bool invertCameraY = false; private int invertY = 1; // Y needs to be flipped, 1: inverted, -1: not inverted
    public bool invertCameraX = false; private int invertX = 1; // X is fine however, -1: inverted, 1: not inverted
    [Range(1, 10)]
    public float cameraSensitivityY = 3;
    [Range(1, 10)]
    public float cameraSensitivityX = 3;
    public bool firstPersonToggle = false;

    [Header("Movement Settings")]
    public float baseSpeed = 3;
    public float jumpVelocity = 3;

    
    [Header("Shooting Settings")]
    public GameObject shot;
    public float fireRate = 1;

    [Header("Object References")]
    public Transform shotTransform;
    public GameObject inventoryHolder;
    public Transform weaponTransform;
    private Inventory inventory;


    // Private variables
    private float verticalVelocity = 0;
    private float nextFire = 0;

    public Vector3 speed { get; private set; }

    // Camera y angle
    private float y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        inventory = inventoryHolder.GetComponent<Inventory>();

        inventory.ItemAdded += ItemAdded;
        inventory.ItemSwitched += ItemSwitched;
        inventory.ItemRemoved += ItemUsed;
    }

    public void ItemAdded(object sender, InventoryEventArgs e)
    {
        GameObject model = e.item.model;
        // Remove child
        if (weaponTransform.transform.childCount > 0)
        {
            Destroy(weaponTransform.GetChild(0).gameObject);
        }

        GameObject weaponModel = Instantiate(model.gameObject,
            weaponTransform.position,
            Camera.main.transform.rotation);
        weaponModel.transform.localScale = model.transform.localScale * model.transform.parent.localScale.x * 0.5f;
        weaponModel.transform.SetParent(weaponTransform);
    }

    public void ItemSwitched(object sender, InventoryEventArgs e)
    {      
        
        GameObject model = e.item.model;
        // Remove child
        if (weaponTransform.transform.childCount > 0)
        {
            Destroy(weaponTransform.GetChild(0).gameObject);
        }

        GameObject weaponModel = Instantiate(model.gameObject,
            weaponTransform.position,
            Camera.main.transform.rotation);
        weaponModel.transform.localScale = model.transform.localScale * model.transform.parent.localScale.x * 0.5f;
        weaponModel.transform.SetParent(weaponTransform);
        
    }

    void ItemUsed(object sender, InventoryEventArgs e)
    {
        if (weaponTransform.transform.childCount > 0)
        {
            Destroy(weaponTransform.GetChild(0).gameObject);
        }
    }

    private void OnDestroy()
    {
        inventory.ItemAdded -= ItemAdded;
        inventory.ItemSwitched -= ItemSwitched;
        inventory.ItemRemoved -= ItemUsed;
    }


    // Update is called once per frame
    void Update()
    {
        if (firstPersonToggle)
        {
            // First person camera

            // rotate the player object about the Y axis
            float rotation = Input.GetAxis("Mouse X");
            rotation *= cameraSensitivityX * invertX;
            transform.Rotate(0, rotation, 0);

            // rotate the camera (the player's "head") about its X axis
            float updown = Input.GetAxis("Mouse Y");
            updown *= cameraSensitivityY * invertY;
            Camera.main.transform.Rotate(updown, 0, 0);
        } 
        else if (!firstPersonToggle)
        {
            // Third person camera

            float rotation = Input.GetAxis("Mouse X");
            transform.Rotate(0, 5.0f * rotation, 0);
            float updown = Input.GetAxis("Mouse Y") * invertY;
            // clamp allowed rotation to 30
            if (y + updown > 60 || y + updown < 0)
            {
                updown = 0;
            }
            y += updown;
            Camera.main.transform.RotateAround(transform.position,
            transform.right,
            updown);
            Camera.main.transform.LookAt(transform);
        }

        // moving forwards and backwards
        float forwardSpeed = Input.GetAxis("Vertical");
        // moving left to right
        float lateralSpeed = Input.GetAxis("Horizontal");
        // apply gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        CharacterController characterController = GetComponent<CharacterController>();

        if (characterController.isGrounded) {
            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpVelocity;
            } else
            {
                // Reset vertical velocity
                verticalVelocity = 0;
            }
        }
        forwardSpeed *= baseSpeed;
        lateralSpeed *= baseSpeed;

        speed = new Vector3(lateralSpeed, verticalVelocity, forwardSpeed);
        // transform this absolute speed relative to the player's current rotation
        // i.e. we don't want them to move "north", but forwards depending on where
        // they are facing
        speed = transform.rotation * speed;
        // multiply by deltaTime to make up for variable framerates
        characterController.Move(speed * Time.deltaTime);        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when the player hits a trigger collider

        // Check if the collider is a pickupable item
        IInventoryItem item = other.gameObject.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.addItem(item);
        }
    }

    void OnValidate()
    {
        jumpVelocity = Mathf.Max(jumpVelocity, 0);

        invertY = invertCameraY ? 1 : -1;
        invertX = invertCameraX ? -1 : 1;
    }

}