using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyToAdvanceHint : MonoBehaviour
{
    public string KeyName;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyName))
        {
            HUDHints.Instance.waitHintFlag = true;
        }
    }
}
