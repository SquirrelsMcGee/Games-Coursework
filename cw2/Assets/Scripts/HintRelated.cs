using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRelated : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetPosition()
    {
        transform.SetPositionAndRotation(position, Quaternion.LookRotation(rotation)); 
    }

    public void DestroyAfterTime(int delay)
    {
        Destroy(gameObject, delay);
    }
}
