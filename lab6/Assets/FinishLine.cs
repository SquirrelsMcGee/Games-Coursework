using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinishLine : MonoBehaviour
{
    public GameObject finishText;

    public void Start()
    {
        finishText.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Play has reached the finish line");
            finishText.SetActive(true);
        }
    }
}
