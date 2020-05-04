using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 0.5f;
    private float _speed = 0;
    private Rigidbody r;

    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;

    public float xMin = -10, xMax = 10, zMin = -10, zMax = 10;

    public GameObject shot;
    public Transform shotTransform;

    public float fireRate = 0.5f;
    private float nextFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement= Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        //Debug.Log("Input: " + horizontalMovement + " " + verticalMovement);
        _speed = speed + (GameController.Instance.waveIndex + 1);
        r.velocity = new Vector3(horizontalMovement * _speed, 0.0f, verticalMovement * _speed);

        r.position = new Vector3(
            Mathf.Clamp(r.position.x, xMin, xMax),
            r.position.y,
            Mathf.Clamp(r.position.z, zMin, zMax));

        if (Input.GetButton("Fire1") && Time.time > nextFire) 
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotTransform.position, shotTransform.rotation);
        }
    }
}
