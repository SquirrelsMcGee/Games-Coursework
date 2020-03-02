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

    [Header("Movement Settings")]
    public float baseSpeed = 3;
    public float jumpVelocity = 3;

    [Header("Shooting Settings")]
    public GameObject shot;
    public float fireRate = 1;

    [Header("Object References")]
    public Transform shotTransform;


    // Private variables
    private float verticalVelocity = 0;
    private float nextFire = 0;




    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        //Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
        //Cursor.lockState = CursorLockMode.None;     // set to default default

    }


    // Update is called once per frame
    void Update()
    {
        // rotate the player object about the Y axis
        float rotation = Input.GetAxis("Mouse X");
        rotation *= cameraSensitivityX * invertX;
        transform.Rotate(0, rotation, 0);
        
        
        // rotate the camera (the player's "head") about its X axis
        float updown = Input.GetAxis("Mouse Y");
        updown *= cameraSensitivityY * invertY;        
        Camera.main.transform.Rotate(updown, 0, 0);


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

        Vector3 speed = new Vector3(lateralSpeed, verticalVelocity, forwardSpeed);
        // transform this absolute speed relative to the player's current rotation
        // i.e. we don't want them to move "north", but forwards depending on where
        // they are facing
        speed = transform.rotation * speed;
        // what is deltaTime?
        // move at a different speed to make up for variable framerates
        characterController.Move(speed * Time.deltaTime);

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot,
            shotTransform.position,
            Camera.main.transform.rotation);
        }
    }

    void OnValidate()
    {
        jumpVelocity = Mathf.Max(jumpVelocity, 0);

        invertY = invertCameraY ? 1 : -1;
        invertX = invertCameraX ? -1 : 1;
    }
}