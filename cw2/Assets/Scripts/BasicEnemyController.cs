using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(NavMeshAgent))]
public class BasicEnemyController : MonoBehaviour
{

    NavMeshAgent agent;

    public Transform targetPosition;
    public Vector3 destination;

    public Transform shotTransform;
    public GameObject bulletPrefab;

    //public TextMeshPro textUI;
    public TextMeshProUGUI textUI;

    public float fireRate = 1.0f;
    private float deltaTime = 0;

    public int health = 0;
    public int maxHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= fireRate) {
                deltaTime = 0;
                Instantiate(bulletPrefab, shotTransform.position, shotTransform.rotation);
            }

            transform.LookAt(targetPosition);
        }

        
        textUI.text = health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            agent.isStopped = true;
        }
    }
}
