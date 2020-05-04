using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDHints : MonoBehaviour
{

    public TextMeshProUGUI hintText;
    public GameObject hintPanel;

    public HintsDataManager hintsData;

    private HintItem currentHint;
    private GameObject currentAppearObject;

    [HideInInspector]
    public bool waitHintFlag = false;

    public static HUDHints Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (hintsData.hints[0] != null) currentHint = hintsData.hints[0];

        hintPanel.SetActive(false);
        StartCoroutine(HintsLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HintsLoop()
    {
        for (int hintIndex = 0; hintIndex < hintsData.hints.Length; hintIndex++)
        {
            hintPanel.SetActive(true);
            currentHint = hintsData.hints[hintIndex];

            GameObject obj = null; // gameobject to instantiate when hint appears
            if (currentHint.appearObject != null) obj = Instantiate(currentHint.appearObject);

            // set position etc. maybe

            if (currentHint.type == HintType.Timed)
            {
                hintText.text = currentHint.hintText;
                yield return new WaitForSeconds(currentHint.time);
            } 
            else if (currentHint.type == HintType.Wait)
            {
                hintText.text = currentHint.hintText;
                
                yield return new WaitUntil(() => waitHintFlag);
                waitHintFlag = false;
            }

            // destroy game object on hint disappear
            if (obj != null) Destroy(obj);
            obj = null;
        }

        hintPanel.SetActive(false);
        yield return null;
    }


}
