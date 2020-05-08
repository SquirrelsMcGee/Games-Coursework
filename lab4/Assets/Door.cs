using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public void Open()
    {
        Debug.Log("opening");
        StartCoroutine(DoorAnimation(-90, 3));
    }

    public void Close()
    {
        Debug.Log("closing");
        StartCoroutine(DoorAnimation(0, 3));
    }

    private IEnumerator DoorAnimation(int targetAngle, int animationSpeed)
    {
        for (int r = 0; r < animationSpeed; r += 1)
        {
            transform.GetChild(0).localEulerAngles = new Vector3(0,
            Mathf.LerpAngle(transform.GetChild(0).localEulerAngles.y, targetAngle,
            5f / animationSpeed),
            0);
            yield return null;
        }
    }
}
