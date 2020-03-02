using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnerController : MonoBehaviour
{

    public int maxSpawn = 1;
    public int spawnDelay = 1;
    public GameObject[] targetTemplates;
    public Vector3 targetScale = Vector3.one;

    private int alreadySpawned = 0;

    private GameObject targetChild = null;

    // Creates targets until its max allocated has been reached, or no available targets remain
    void Awake()
    {
        CreateTarget();
    }

    // Update called each frame
    void Update()
    {
        if (targetChild == null && alreadySpawned < maxSpawn)
        {
            Invoke("CreateTarget", spawnDelay);
        }
    }


    // If the TargetSpawner does not already have a child, and the maximum allocated targets has not been reached
    // Create a new target
    // Then append it to the TargetSpawner, and set its scale and position accordingly
    void CreateTarget()
    {
        if (targetChild == null && alreadySpawned < maxSpawn)
        {
            GameObject newTarget = targetTemplates[Random.Range(0, targetTemplates.Length)];
            targetChild = Instantiate(newTarget);
            
            targetChild.transform.parent = gameObject.transform;
            targetChild.transform.localScale = targetScale;
            targetChild.transform.localPosition = Vector3.zero;

            alreadySpawned += 1;
        }
    }

    // For debugging, draw a red cube in place of the target
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, targetScale);
    }
}
