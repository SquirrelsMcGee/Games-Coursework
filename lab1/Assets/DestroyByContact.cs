using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    // Start is called before the first frame update

    GameController gameController;

    GameObject playerShip;
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        playerShip = GameObject.Find("Player");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroys self and the colliding object on collision
    void OnTriggerEnter(Collider other)
    {
        gameController.AddScore(10);

        Destroy(other.gameObject);
        Destroy(gameObject);

        if (other.gameObject == playerShip) { Application.LoadLevel(Application.loadedLevel); }
    }
}
