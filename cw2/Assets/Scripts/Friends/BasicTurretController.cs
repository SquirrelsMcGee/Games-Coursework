using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretController : MonoBehaviour
{

    public GameObject bulletPrefab;

    public Transform weaponTransform;
    public Transform[] shotTransform;

    public float fireRate = 1.0f;
    protected float deltaTime = 0.0f;

    public float range = 10.0f;

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        GameObject targetEnemy = FindClosestEnemy();

        if (targetEnemy == null)
        {
            // Set state to idle
            
            return;
        }
        Vector3 diff = targetEnemy.transform.position - transform.position;
        float dist = diff.sqrMagnitude;

        if (dist > Mathf.Pow(range, 2))
        {
            // out of range rip
            return;
        }

        weaponTransform.LookAt(targetEnemy.transform);

        deltaTime += Time.deltaTime;

        if (deltaTime >= fireRate)
        {
            deltaTime = 0;
            GameObject bullet = Instantiate(bulletPrefab, shotTransform[0].position, shotTransform[0].rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
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
