using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnerController : MonoBehaviour
{

    public int maxSpawn = 1;
    public int spawnDelay = 1;
    public GameObject targetTemplate;
    public Vector3 targetScale = new Vector3(1, 1, 1);

    private int alreadySpawned = 0;

    private GameObject targetChild = null;

    // Creates targets until its max allocated has been reached, or no available targets remain
    void Awake()
    {
        CreateTarget();
    }

    void Update()
    {
        if (targetChild == null && alreadySpawned < maxSpawn)
        {
            Invoke("CreateTarget", spawnDelay);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, targetScale);
    }

    void CreateTarget()
    {
        if (targetChild == null && alreadySpawned < maxSpawn)
        {
            targetChild = Instantiate(targetTemplate);
            
            targetChild.transform.parent = gameObject.transform;
            targetChild.transform.localScale = targetScale;
            targetChild.transform.localPosition = Vector3.zero;

            alreadySpawned += 1;
        }

    }
}
