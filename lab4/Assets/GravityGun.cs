using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform holdPosition;
    public LayerMask layerMask;
    private GameObject heldObject;
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (heldObject == null)
            {
                RaycastHit colliderHit;
                if (Physics.Raycast(transform.position, transform.forward, out colliderHit, 10.0f, layerMask))
                {
                    // pick up the object 
                    heldObject = colliderHit.collider.gameObject;
                    
                    heldObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
            else if (heldObject != null)
            {
                // drop the object again
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().ResetInertiaTensor();
                heldObject = null;
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (heldObject != null)
            {
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10.0f, ForceMode.Impulse);
                heldObject = null;
                
            }
        }


        if (heldObject != null)
        {
            // move the thing we're holding
            heldObject.GetComponent<Rigidbody>().MovePosition(holdPosition.position);
            heldObject.GetComponent<Rigidbody>().MoveRotation(holdPosition.rotation);
        }
    }
}
