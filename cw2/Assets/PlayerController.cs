using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public LayerMask layerMask;

    public float angleScale = 1.0f;
    public float placeableDistance = 1.0f;

    public int buildCost = 1;

    public GameObject turretPrefab;

    private Vector3 vectorPointA;
    private Vector3 vectorPointB;
    private Vector3 vectorDirectionY;
    private Vector3 vectorDirectionAngle;
    private Vector3 placePosition;

    public bool placeable = false;

    private float deltaTime = 0;
    private float inputRate = 1;

    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // raycast forward and down to the floor to see if the player can place an object
        CalculatePlacePosition();
        GetMouseButtonInput();
    }

    void GetMouseButtonInput()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= inputRate)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Taking input");
                TryBuilding();
            }
        }
    }

    void TryBuilding()
    {
        if (placeable)
        {
            // If player can place an object
            // Check if player has the funds

            Debug.Log(gameController.achievedScore + " " + gameController.usedScore);
            if (gameController.achievedScore > gameController.usedScore + buildCost)
            {
                Instantiate(turretPrefab, placePosition, transform.rotation);
                gameController.usedScore += buildCost;
            }
        }
    }

    void CalculatePlacePosition()
    {
        vectorPointA = transform.position + (transform.forward * angleScale);
        vectorDirectionY = (transform.up * -1);
        vectorPointB = vectorPointA + vectorDirectionY;

        vectorDirectionAngle = transform.position - vectorPointB;

        Ray ray = new Ray(transform.position, -vectorDirectionAngle);
        RaycastHit hitInfo;
        placeable = Physics.Raycast(ray, out hitInfo, placeableDistance, layerMask);
        if (placeable)
        {
            placePosition = hitInfo.point;
        }
    }

    private void OnDrawGizmos()
    {
        if (placePosition == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, placePosition);
        Gizmos.DrawSphere(placePosition, 0.2f);
        /*
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, vectorPointA);
        Gizmos.DrawSphere(vectorPointA, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, vectorPointA + vectorDirectionY);
        //Gizmos.DrawSphere(vectorPointA + vectorDirectionY, 0.2f);
        */

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - vectorDirectionAngle);
    }


}
