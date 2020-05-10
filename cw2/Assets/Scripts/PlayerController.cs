using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides transform and Vector3 position data for showing item previews and weaponry
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Layer where sentries can be places")]
    public LayerMask buildzoneLayer;

    [Header("Variables for sentry positioning")]
    [Tooltip("How far forward should turrets be placed")]
    public float forwardDistance = 1.0f;
    [Tooltip("The maximum angled distance between the player and the place position")]
    public float angledDistance = 1.0f;

    [Header("Weapon Transforms")]
    [Tooltip("Transform where bullets should appear")]
    public Transform shotTransform;
    [Tooltip("Transform where weapons should appear")]
    public Transform weaponTransform;

    // The position where any preview turrets should be positioned
    public Vector3 placePosition { get; private set; }

    [HideInInspector]
    // If the player has a valid turret place position and is not jumping
    public bool turretPlaceable = false;

    // Static singleton reference
    public static PlayerController Instance { get; private set; }

    // Vectors for calculating the turret preview position
    // Also used for drawing debug Gizmos
    private Vector3 vectorPointA;
    private Vector3 vectorPointB;
    private Vector3 vectorDirectionY;
    private Vector3 vectorDirectionAngle;

    private void Awake()
    {
        // Create singleton reference
        if (Instance == null) Instance = this;
        else { Destroy(gameObject);  }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the place position on every frame update
        CalculatePlacePosition();
    }

    void CalculatePlacePosition()
    {
        // Get a point in front of the player at a given distance
        vectorPointA = transform.position + (transform.forward * forwardDistance);

        // Get a negative vertical vector
        vectorDirectionY = (transform.up * -1);

        // Calculate the vector below the frontal point
        vectorPointB = vectorPointA + vectorDirectionY;

        // Calculate the vector between the player's position, and vector position
        // This gives a vector describing an angled direction
        vectorDirectionAngle = transform.position - vectorPointB;

        // Raycast along this diration
        Ray ray = new Ray(transform.position, -vectorDirectionAngle);
        RaycastHit hitInfo;

        // If the ray cast hit anything
        turretPlaceable = Physics.Raycast(ray, out hitInfo, angledDistance, buildzoneLayer);
        if (turretPlaceable)
        {
            // Set the turret place position to the point where the raycast hit
            placePosition = hitInfo.point;
        }

        // To create a "no build" zone, we use a Trigger collider spanning the whole area where we don't want the player to build
        // If the raycast hit any trigger collider, set the placeable flag to false
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.isTrigger) turretPlaceable = false;
        }
    }

    /// <summary>
    /// Used for debugging the placePosition calculation
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.forward * 1.5f);

        if (shotTransform != null) Gizmos.DrawSphere(shotTransform.position, 0.1f);
        if (weaponTransform != null) Gizmos.DrawSphere(weaponTransform.position, 0.1f);

        if (placePosition == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, placePosition);
        Gizmos.DrawSphere(placePosition, 0.2f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, vectorPointA);
        Gizmos.DrawSphere(vectorPointA, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, vectorPointA + vectorDirectionY);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - vectorDirectionAngle);
    }


}
