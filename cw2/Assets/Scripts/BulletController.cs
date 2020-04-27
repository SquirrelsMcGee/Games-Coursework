using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed = 5.0f;
    private Rigidbody r;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.velocity = transform.forward * speed;

        // Destroys self after 20 seconds
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Destroy self after collision
    void OnCollisionEnter(Collision collision)
    {

        //Debug.Log(collision.gameObject.tag);
        // Fix location
        r.constraints = RigidbodyConstraints.FreezePosition;

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<BasicEnemyController>().health -= 1;
                break;
            case "Target":
                collision.gameObject.GetComponent<TargetController>().health -= 1;
                break;
            case "Shield":
                collision.gameObject.GetComponent<ShieldController>().health -= 1;
                break;
        }


        // Destroy self
        Destroy(gameObject);
    }

}
