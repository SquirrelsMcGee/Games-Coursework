using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    private bool selfDestruct = false;
    private Vector3 scale;

    private GameController gc;

    public float shrinkFactor = 0.9f;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameDirector").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selfDestruct)
        {
            scale = transform.localScale;
            scale -= Vector3.one * (Time.deltaTime * shrinkFactor);
            transform.localScale = scale;

            if (transform.localScale.x <= 0.01)
            {
                Destroy(gameObject);
                gc.UpdateScore();
                gc.UpdateHits();
            }

        }
    }

    // Destroy self after collision
    // For use with the Bullet prefab, ensure that the object is in the "Bullet" layer (layer id = 10)
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.gameObject.layer) ;
        if (collision.collider.gameObject.layer == 10)
        {
            selfDestruct = true;
        }
    }
}
