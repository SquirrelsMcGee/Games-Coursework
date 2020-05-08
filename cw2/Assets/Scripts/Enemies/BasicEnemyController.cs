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

    public Animator animator;

    public TextMeshProUGUI textUI;

    public int damage = 10;
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

            transform.LookAt(new Vector3(targetPosition.position.x, transform.position.y, targetPosition.position.z));
            weaponTransform.LookAt(targetPosition);
        }

        
        textUI.text = health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            StartCoroutine(DelayedDestroy());
        }
    }

    protected virtual void TimedUpdate()
    {
        if (animator != null) animator.SetBool("idle", true);
        if (animator != null) animator.SetBool("atk", false);

        deltaTime += Time.deltaTime;
        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            GameObject bullet = Instantiate(bulletPrefab, shotTransform.position, shotTransform.rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
            bullet.GetComponent<BulletController>().damage = damage;
            if (animator != null) animator.SetBool("idle", false);
            if (animator != null) animator.SetBool("atk", true);
            
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
            if (animator != null) animator.SetBool("idle", false);
        }
    }

    protected virtual void OnDestroy()
    {
        GameController.Instance.AddScore(1);
        GameController.Instance.GameEventLoss -= OnGameEnd;
        GameController.Instance.GameEventLoss -= OnGameLoss;
        GameController.Instance.GameEventLoss -= OnGameWin;

        print("Achieved Score: " + GameController.Instance.achievedScore);
    }

    IEnumerator DelayedDestroy()
    {
        if (animator != null) animator.SetBool("atk", false);
        if (animator != null) animator.SetBool("idle", false);
        if (animator != null) animator.SetBool("die", true);

        gameObject.layer = 0;
        GetComponent<CapsuleCollider>().enabled = false;
        //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSeconds(0);
        Destroy(gameObject);
    }

    protected virtual void OnGameEnd(object s, GameEventArgs e)
    {
        Destroy(gameObject);
    }

    protected virtual void OnGameWin(object s, GameEventArgs e)
    {
        // Show Game Win Screen
    }

    protected virtual void OnGameLoss(object s, GameEventArgs e)
    {
        // Show Game Loss Screen
    }
}
