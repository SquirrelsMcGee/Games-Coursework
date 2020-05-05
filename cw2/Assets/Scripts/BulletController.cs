using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public LayerMask parentLayerMask;

    public float speed = 5.0f;

    public int damage = 1;
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

        if (collision.gameObject.layer == parentLayerMask)
        {
            // prevent friendly-fire
            Destroy(gameObject);
            return;
        }
        Debug.Log(collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<BasicEnemyController>().health -= damage;
                break;
            case "Target":
                collision.gameObject.GetComponent<TargetController>().health -= damage;
                break;
            case "Shield":
                collision.gameObject.GetComponent<ShieldController>().health -= damage;
                break;
        }

        // Destroy self
        // Bullets automatically get destroyed upon timeout,
        // so prevent them from colliding with terrain
        if (gameObject.layer != LayerMask.GetMask("BuildZone")) Destroy(gameObject);
    }

}
