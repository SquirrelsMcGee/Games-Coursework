using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


/// <summary>
/// Basic enemy behaviour
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BasicEnemyController : MonoBehaviour
{


    [Header("Object References")]
    public Transform weaponTransform;
    public Transform shotTransform;
    public GameObject bulletPrefab;
    public TextMeshProUGUI textUI;

    [Header("Animation Control")]
    public Animator animator;

    [Header("Behaviour Settings")]
    [Tooltip("How much damage the enemy does on hit")]
    public int damage = 1;
    [Tooltip("How fast the enemy can fire")]
    public float fireRate = 1.0f;

    [Tooltip("The maximum health the enemy spawns with")]
    public int maxHealth = 5;

    [HideInInspector]
    // How much health the enemy has
    public int health = 0;

    [HideInInspector]
    // The Transform of the object the enemy is trying to get to
    public Transform targetPosition;

    [HideInInspector]
    // Vector3 position of the above transform
    public Vector3 destination;

    // Agent for navmesh navigation
    protected NavMeshAgent agent;
    // Duration since last Timed Update
    protected float deltaTime = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Set the target destination for the enemy to navigate to
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination;

        // Set the health value
        InitHealth();

        // Subscribe the the End game event
        GameController.Instance.GameEventEnd += OnGameEnd;
    }

    /// <summary>
    /// Sets the current health value to the max health value
    /// </summary>
    public void InitHealth()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        // if the agent is stopped, then it is near the target
        if (agent.isStopped)
        {
            // Fire bullets are the fire rate
            TimedUpdate();

            // Aim at the target
            transform.LookAt(new Vector3(targetPosition.position.x, transform.position.y, targetPosition.position.z));
            weaponTransform.LookAt(targetPosition);
        }

        // Display the current and max health above the enemy
        textUI.text = health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            StartCoroutine(DelayedDestroy());
        }
    }

    /// <summary>
    /// Updates when deltaTime >= fireRate
    /// </summary>
    protected virtual void TimedUpdate()
    {
        // Set the current animation to idle
        if (animator != null) animator.SetBool("idle", true);
        if (animator != null) animator.SetBool("atk", false);

        deltaTime += Time.deltaTime;

        // If the fire rate duration has passed since the last time this section ran
        if (deltaTime >= fireRate)
        {
            // Reset the delta time
            deltaTime = 0;

            // Instantiate a bullet, and set its physics settings so it cannot damage other enemies
            GameObject bullet = Instantiate(bulletPrefab, shotTransform.position, shotTransform.rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
            bullet.GetComponent<BulletController>().damage = damage;

            // Set the current animation to attacking
            if (animator != null) animator.SetBool("idle", false);
            if (animator != null) animator.SetBool("atk", true);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        // If sub-class enemies wish to wait after reaching the target before stopping
        StartCoroutine(DelayedTrigger(other, 0.0f));
    }

    IEnumerator DelayedTrigger(Collider other, float delay)
    {
        // Wait for the provided time
        yield return new WaitForSeconds(delay);

        // Check if the enemy has reached the destination trigger collider
        if (other.gameObject.CompareTag("Target"))
        {

            // Stop the agent
            agent.isStopped = true;
            if (animator != null) animator.SetBool("idle", false);
        }
    }

    /// <summary>
    /// Updates the game score when the enemy is destroyed
    /// </summary>
    protected virtual void OnDestroy()
    {
        GameController.Instance.AddScore(1);
        GameController.Instance.GameEventEnd -= OnGameEnd;

        print("Achieved Score: " + GameController.Instance.achievedScore);
    }

    /// <summary>
    /// [Unused] Waits for the current animation to finish before destroying the enemy
    /// </summary>
    /// <returns></returns>
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

    // Destroys seld on when GameEventEnd is broadcast
    protected virtual void OnGameEnd(object s, GameEventArgs e)
    {
        Destroy(gameObject);
    }
}
