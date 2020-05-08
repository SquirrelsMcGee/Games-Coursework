using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public GameObject finishText;
    // Start is called before the first frame update
    void Start()
    {
        finishText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            finishText.SetActive(true);
        }
    }
}
