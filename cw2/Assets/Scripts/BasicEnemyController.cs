using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


[RequireComponent(typeof(NavMeshAgent))]
public class BasicEnemyController : MonoBehaviour
{
    [HideInInspector]
    public Transform targetPosition;
    [HideInInspector]
    public Vector3 destination;

    public Transform weaponTransform;
    public Transform shotTransform;
    public GameObject bulletPrefab;

    public TextMeshProUGUI textUI;

    public float fireRate = 1.0f;
    [HideInInspector]
    public int health = 0;
    public int maxHealth = 5;

    protected NavMeshAgent agent;
    protected float deltaTime = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination;

        health = maxHealth;

        GameController.Instance.GameEventLoss += OnGameEnd;
        GameController.Instance.GameEventLoss += OnGameLoss;
        GameController.Instance.GameEventLoss += OnGameWin;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (agent.isStopped)
        {
            TimedUpdate();

            weaponTransform.LookAt(targetPosition);
        }

        
        textUI.text = health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            Destroy(gameObject);
        }
    }

    protected virtual void TimedUpdate()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            Instantiate(bulletPrefab, shotTransform.position, shotTransform.rotation);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DelayedTrigger(other, 0.0f));
    }

    IEnumerator DelayedTrigger(Collider other, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (other.gameObject.CompareTag("Target"))
        {
            agent.isStopped = true;
        }
    }

    protected virtual void OnDestroy()
    {
        GameController.Instance.achievedScore += 1;
        GameController.Instance.GameEventLoss -= OnGameEnd;
        GameController.Instance.GameEventLoss -= OnGameLoss;
        GameController.Instance.GameEventLoss -= OnGameWin;
    }

    protected virtual void OnGameEnd(object s, GameEventArgs e)
    {
        Destroy(gameObject);
    }

    protected virtual void OnGameWin(object s, GameEventArgs e)
    {
        // Stub
    }

    protected virtual void OnGameLoss(object s, GameEventArgs e)
    {
        // Stub
    }
}
