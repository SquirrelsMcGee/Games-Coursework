using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetController : MonoBehaviour
{

    [HideInInspector]
    public int maxHealth = 0;

    [HideInInspector]
    public int health = 0;

    public TextMeshProUGUI textUI;

    public static TargetController Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

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


        textUI.text = health + "/" + maxHealth;
    }
}
