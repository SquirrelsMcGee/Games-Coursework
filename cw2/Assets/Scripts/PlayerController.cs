using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Layer where sentries can be places")]
    public LayerMask buildzoneLayer;

    [Header("Variables for sentry positioning")]
    public float forwardDistance = 1.0f;
    public float angledDistance = 1.0f;
    
    private Vector3 vectorPointA;
    private Vector3 vectorPointB;
    private Vector3 vectorDirectionY;
    private Vector3 vectorDirectionAngle;
    public Vector3 placePosition { get; private set; } // Needs to be public so that the placeable item scripts can access it

    [HideInInspector]
    public bool turretPlaceable = false;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject);  }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // raycast forward and down to the floor to see if the player can place an object
        CalculatePlacePosition();
    }

    void CalculatePlacePosition()
    {
        vectorPointA = transform.position + (transform.forward * forwardDistance);
        vectorDirectionY = (transform.up * -1);
        vectorPointB = vectorPointA + vectorDirectionY;

        vectorDirectionAngle = transform.position - vectorPointB;

        Ray ray = new Ray(transform.position, -vectorDirectionAngle);
        RaycastHit hitInfo;
        turretPlaceable = Physics.Raycast(ray, out hitInfo, angledDistance, buildzoneLayer);
        if (turretPlaceable)
        {
            placePosition = hitInfo.point;
        }
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.isTrigger) turretPlaceable = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (placePosition == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, placePosition);
        Gizmos.DrawSphere(placePosition, 0.2f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, vectorPointA);
        Gizmos.DrawSphere(vectorPointA, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, vectorPointA + vectorDirectionY);
        //Gizmos.DrawSphere(vectorPointA + vectorDirectionY, 0.2f);
        

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - vectorDirectionAngle);
    }


}
