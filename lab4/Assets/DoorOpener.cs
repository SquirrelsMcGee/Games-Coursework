using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    public Door doorObject;

    private void OnTriggerEnter(Collider other)
    {
        doorObject.Open();
        
    }
    private void OnTriggerExit(Collider other)
    {
        doorObject.Close();
    }
}
