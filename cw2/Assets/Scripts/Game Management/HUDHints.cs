using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDHints : MonoBehaviour
{

    /*
     * Public Variables
    */
    // Scene References
    [Header("Used for displaying hints to the player", order = 1)]
    public TextMeshProUGUI hintText;
    public GameObject hintPanel;

    // Data References
    [Header("Hints Data References", order = 2)]
    public HintsDataManager hintsData;

    // Script settings
    [Header("Script Settings", order = 3)]
    public bool SkipTutorial = false;

    // This flag is used to have the script wait for an event before advancing the hints
    [HideInInspector]
    public bool waitHintFlag = false;

    // Static singleton reference
    public static HUDHints Instance { get; private set; }

    /*
     * Private Variables
    */

    private HintItem currentHint; // Currently shown hint
    private GameObject currentAppearObject; // Reference to object that was created with the hint

    // Create reference to singleton
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set first hint
        if (hintsData.hints[0] != null) currentHint = hintsData.hints[0];

        // Disable the hints panel by default
        hintPanel.SetActive(false);

        // Start displaying hints
        StartCoroutine(HintsLoop());
    }

    IEnumerator HintsLoop()
    {
        // For each hint in the list
        for (int hintIndex = 0; hintIndex < hintsData.hints.Length; hintIndex++)
        {

            // Enable the hint panel
            hintPanel.SetActive(true);

            // Set the current hint reference
            currentHint = hintsData.hints[hintIndex];

            // If the hint has an object to create
            if (currentHint.appearObject != null)
            {
                // Instantiate it and create a reference to the instance
                currentAppearObject = Instantiate(currentHint.appearObject);
            }

            // Different behaviour for different hint types
            if (currentHint.type == HintType.Timed)
            {
                // Display Timed hints for the given number of seconds
                hintText.text = currentHint.hintText;
                if (!SkipTutorial) yield return new WaitForSeconds(currentHint.time);
            } 
            else if (currentHint.type == HintType.Wait)
            {
                // Display Wait hints until the wait flag is enabled
                hintText.text = currentHint.hintText;
                yield return new WaitUntil(() => waitHintFlag);
                waitHintFlag = false;
            }

            // Destroy instance of created object when the hint is no longer visible
            if (currentAppearObject != null) Destroy(currentAppearObject);
            currentAppearObject = null;
        }

        // After all hints have been displayed, hide the panel and begin the level
        hintPanel.SetActive(false);
        EnemySpawner.Instance.StartLevel();
        yield return null;
    }
}
