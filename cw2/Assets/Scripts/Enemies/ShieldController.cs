using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Provides the ShieldEnemy with a destructable shield
/// </summary>
public class ShieldController : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("Where the shield health should be displayed")]
    public TextMeshProUGUI textUI;

    [Header("Behaviour Settings")]
    [Tooltip("The maximum health the shield spawns with")]
    public int maxHealth = 5;

    [HideInInspector]
    // How much health the shield current has
    public int health = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise health
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If the health depletes to 0, destroy the shield
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // Display health as text
        textUI.text = health + "/" + maxHealth;
    }
}
