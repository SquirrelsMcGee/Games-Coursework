using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetController : MonoBehaviour
{

    
    [Header("Object References")]
    [Tooltip("Displays the defence point health in world space")]
    public TextMeshProUGUI textUI;

    [Header("Appearance")]
    [Tooltip("Used to modify the appearance of the defence point based on its health")]
    public Material material;

    [HideInInspector]
    // The defence point's maximum health
    public int maxHealth = 0;

    [HideInInspector]
    // The defence point's current health
    public int health = 0;

    // Static singleton reference
    public static TargetController Instance { get; private set; }

    private void Awake()
    {
        // Create singleton reference
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start() { }

    /// <summary>
    /// Initialises the maximum and current health values to the given number
    /// </summary>
    /// <param name="h">Health value</param>
    public void InitHealth(int h)
    {
        maxHealth = h;
        health = h;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // Destroy self when health hits 0
            //Destroy(gameObject);
            GameController.Instance.GameEnd(GameStates.Loss);
            gameObject.SetActive(false);
        }

        // Clamp upper bound of health to maxHealth
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // Calculate the current health ratio
        // This is used for modifiy the appearance of the defence point using a material
        float healthRatio = ((float)TargetController.Instance.health) / ((float)TargetController.Instance.maxHealth);

        // Max health = green, 0 health = red
        Color temp = Color.Lerp(Color.red, Color.green, healthRatio);
        temp.a = 0.5f;
        material.color = temp;

        // Display current health above the defence point
        textUI.text = health + "/" + maxHealth;
    }
}
