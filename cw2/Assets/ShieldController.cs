using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldController : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public int maxHealth = 5;

    [HideInInspector]
    public int health = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        textUI.text = health + "/" + maxHealth;
    }
}
