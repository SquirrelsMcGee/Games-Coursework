﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = transform.forward * speed * (GameController.Instance.waveIndex + 1);
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
}
