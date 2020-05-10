using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for providing bullets with movement
/// </summary>
public class BulletController : MonoBehaviour
{
    // Layer to ignore when performing damage calculations
    // This is the layer that the object which created the bullet is in
    public LayerMask parentLayerMask;

    // Soeed at which the bullet moves
    public float speed = 5.0f;

    // Damage the bullet does on impact
    public int damage = 1;

    // RigidBody for movement
    private Rigidbody r;

    // Start is called before the first frame update
    void Start()
    {
        // Move the bullet forwards at the given speed
        r = GetComponent<Rigidbody>();
        r.velocity = transform.forward * speed;

        // Destroys self after 20 seconds
        Destroy(gameObject, 20);
    }

    // Destroy self after collision
    void OnCollisionEnter(Collision collision)
    {
        // Fix location
        r.constraints = RigidbodyConstraints.FreezePosition;

        // Check to see if the collision object's layer is same as the layer mask of the parent object
        if (collision.gameObject.layer == parentLayerMask)
        {
            // Prevent friendly-fire
            Destroy(gameObject);
            return;
        }

        // Perform damage calculation
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
        // Turrets had issues with shooting the terrain when facing nearby enemies
        // Because the bullets self-destruct on a timer, bullets should pass through the terrain
        if (gameObject.layer != LayerMask.GetMask("BuildZone")) Destroy(gameObject);
    }

}
