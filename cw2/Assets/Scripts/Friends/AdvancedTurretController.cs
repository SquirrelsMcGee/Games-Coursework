using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Advanced turret behaviour, Subclass of BasicTurretController
/// </summary>
public class AdvancedTurretController : BasicTurretController
{
    // Index for shot transform selection
    private int shotIndex = 0;

    // Start is called before the first frame update
    public override void Start() { }

    // Update is called once per frame
    public override void Update()
    {
        // Finds the closest enemy
        GameObject targetEnemy = FindClosestEnemy();

        if (targetEnemy == null)
        {
            // Set state to idle
            return;
        }

        // Checks if the range to the closest enemy is within the range of the turret
        Vector3 diff = targetEnemy.transform.position - transform.position;
        float dist = diff.sqrMagnitude;

        if (dist > Mathf.Pow(range, 2))
        {
            // out of range
            return;
        }
        // If it is within range
        
        // Aim at the enemy
        weaponTransform.LookAt(targetEnemy.transform);

        // Fire bullets at the fire rate
        deltaTime += Time.deltaTime;

        if (deltaTime >= fireRate)
        {
            deltaTime = 0;

            // Uses multiple barrels for bullets to appear from, cycle through these transforms
            shotIndex++;
            if (shotIndex >= shotTransform.Length) shotIndex = 0;

            // Instantiate bullet
            GameObject bullet = Instantiate(bulletPrefab, shotTransform[shotIndex].position, shotTransform[shotIndex].rotation);
            bullet.GetComponent<BulletController>().parentLayerMask = gameObject.layer;
        }
    }
}
