using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : BasicEnemyController
{

    public float stopRange = 10.0f;
    private GameObject targetTurret;

    string prefix = "";

    protected override void Start()
    {
        targetTurret = ChooseRandomTurret();
        if (targetTurret != null)
        {
            targetPosition = targetTurret.transform;
            destination = targetPosition.transform.position;
        }

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (agent.isStopped)
        {
            if (targetPosition != null) {
                if (targetTurret != null)
                {
                    weaponTransform.LookAt(targetPosition);
                }
                else
                {
                    weaponTransform.LookAt(weaponTransform.rotation.eulerAngles + targetPosition.position);
                }
            }
        } 

        TimedUpdate();

        textUI.text = prefix + health + "/" + maxHealth;

        if (health <= 0)
        {
            // Destroy self when health hits 0
            Destroy(gameObject);
        }
    }

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

            if (!agent.isStopped)
            {
                Vector3 diff = targetPosition.transform.position - transform.position;
                float dist = diff.sqrMagnitude;

                if (dist < Mathf.Pow(stopRange, 2))
                {
                    agent.isStopped = true;
                }
            } else
            {
                // Do something when at the target, maybe explode i guess
            }
        }
    }

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
