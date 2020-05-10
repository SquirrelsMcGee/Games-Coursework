using UnityEngine;

/// <summary>
/// Sets the HUDHints wait flag when the given key is detected as being pressed
/// </summary>
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
