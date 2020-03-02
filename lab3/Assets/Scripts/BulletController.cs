using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = transform.forward * speed;


        // Destroys self after 10 seconds
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroy self after collision
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);        
    }
}
