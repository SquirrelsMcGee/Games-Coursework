using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurretController : BasicTurretController
{

    private int shotIndex = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
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
            shotIndex++;
            if (shotIndex >= shotTransform.Length) shotIndex = 0;
            GameObject bullet = Instantiate(bulletPrefab, shotTransform[shotIndex].position, shotTransform[shotIndex].rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
        }
    }
}
