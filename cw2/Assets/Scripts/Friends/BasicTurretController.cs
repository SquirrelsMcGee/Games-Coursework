using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic turret behaviour
/// </summary>
public class BasicTurretController : MonoBehaviour
{
    [Header("Prefab Objects")]
    public GameObject bulletPrefab;

    [Header("Object References")]
    public Transform weaponTransform;
    public Transform[] shotTransform;

    [Header("Behaviour Settings")]
    [Tooltip("How fast the turret can fire")]
    public float fireRate = 1.0f;
    [Tooltip("How far the turret can see")]
    public float range = 10.0f;

    // Duration since last Timed update
    protected float deltaTime = 0.0f;

    // Start is called before the first frame update
    public virtual void Start()
    { }

    // Update is called once per frame
    public virtual void Update()
    {
        GameObject targetEnemy = FindClosestEnemy();

        if (targetEnemy == null)
        {
            // Do nothing if there are no enemies within range
            return;
        } else
        {
            // If there is an enemy within range, aim towards it
            weaponTransform.LookAt(targetEnemy.transform);

            // Fire bullets at fire rate
            TimedUpdate();
        }

    }

    /// <summary>
    /// Runs when deltaTime >= fireRate
    /// </summary>
    protected virtual void TimedUpdate()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            GameObject bullet = Instantiate(bulletPrefab, shotTransform[0].position, shotTransform[0].rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
        }
    }

    /// <summary>
    /// Returns the closest enemy on the map. Returns null if there are none, or if they are out of the Turret's range
    /// </summary>
    public GameObject FindClosestEnemy()
    {

        /*
         * This function was based off the example given in the Unity ScriptReference documentation
         * https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
        */
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemy objects
        GameObject closest = null; // Reference to the closest enemy

        if (enemyList.Length == 0) return null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        Vector3 diff;

        // Loop through the enemies, comparing square distances to find the closest enemy
        foreach (GameObject enemy in enemyList)
        {
            diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }

        // Check if closest enemy is within range
        diff = closest.transform.position - position;
        float dist = diff.sqrMagnitude;

        
        if (dist > Mathf.Pow(range, 2))
        {
            // If it is out of range, return null for no target
            closest = null;
        }
        
        return closest;
    }
}
