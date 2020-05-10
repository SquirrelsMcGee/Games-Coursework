using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables the cursor on load, this is needed because Unity StandardAssets/FPSController hides the cursor even between scene changes
/// </summary>
public class EnableMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Screen.lockCursor = false;
    }
}
