using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shield Enemy Behaviour, Subclass of BasicEnemyController
/// </summary>
public class ShieldEnemy : BasicEnemyController
{
    [Header("Behaviour Settings")]
    [Tooltip("How close the enemy should get to its destination turret before stopping")]
    public float stopRange = 10.0f;

    // Reference to the turret that the enemy is trying to get to
    private GameObject targetTurret;

    // Prefix for health display
    string prefix = "";

    // Flag for if the enemy spawns in and there are no turrets in the scene
    bool hasTurretTarget = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        // Chooses a random turret on the map
        targetTurret = ChooseRandomTurret();

        // If there is a turret to find
        if (targetTurret != null)
        {
            // Set agent to navigate to this position
            targetPosition = targetTurret.transform;
            destination = targetPosition.transform.position;

            // Sets the turret-as-destination flag to true
            hasTurretTarget = true;
        }

        // Calls base class Start method
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Checks if the turret-as-destination flag is false
        if (!hasTurretTarget)
        {
            // Chooses a random turret on the map
            targetTurret = ChooseRandomTurret();

            // If there is a turret to find
            if (targetTurret != null)
            {
                // Set agent to navigate to this position
                targetPosition = targetTurret.transform;
                destination = targetPosition.transform.position;
                
                // Sets the turret-as-destination flag to true
                hasTurretTarget = true;
            }
        }
        // Otherwise continue towards the defense point

        // If the agent has stopped
        if (agent.isStopped)
        {
            if (targetPosition != null) {

                // Check if the destination was a turret
                if (targetTurret != null)
                {
                    // If so, aim the shield towards the turret to block its bullets
                    weaponTransform.LookAt(targetPosition);
                }
                else
                {
                    // Otherwise, the destination was the defense point
                    // look away to protect other enemies
                    weaponTransform.LookAt(weaponTransform.rotation.eulerAngles + targetPosition.position);
                }
            }
        }


        // Called Timed Update method
        TimedUpdate();

        // Display health
        textUI.text = prefix + health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Updates when deltaTime >= fireRate
    /// </summary>
    protected override void TimedUpdate()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            // Do something

            if (targetPosition == null)
            {
                // Set state to idle
                return;
            }

            // Check if the enemy has reached close to its destination
            if (!agent.isStopped)
            {
                Vector3 diff = targetPosition.transform.position - transform.position;
                float dist = diff.sqrMagnitude;

                if (dist < Mathf.Pow(stopRange, 2))
                {
                    // If so, stop the agent
                    agent.isStopped = true;
                }
            } else
            {
                // Do something when at the target, maybe explode i guess
            }
        }
    }

    /// <summary>
    /// Chooses a random turret on the map. Returns null if no turrets where found
    /// </summary>
    /// <returns>GameObject reference to an object with the "Friendly" tag</returns>
    public GameObject ChooseRandomTurret()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Friendly");
        if (enemyList.Length == 0)
        {
            return null;
        }

        int i = Random.Range(0, enemyList.Length);
        return enemyList[i];
    }

    /// <summary>
    /// Finds the closest turret on the map to the enemy. Returns null if no turrets where found
    /// </summary>
    /// <returns>GameObject reference to an object with the "Friendly" tag</returns>
    public GameObject FindClosestTurret()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Friendly");
        GameObject closest = null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemyList)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}
