using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyByContact : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject playerShip;

    void Start()
    {
        playerShip = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroys self and the colliding object on collision
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);

        GameController.Instance.AddScore(1);

        if (other.gameObject == playerShip) {
            StartCoroutine(delayedEnd());
        }
    }
    IEnumerator delayedEnd()
    {
        GameController.Instance.lose.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
